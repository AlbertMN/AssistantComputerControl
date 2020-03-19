$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

/* TRANSLATION STUFF */
var chosenLanguageSelect = $("#chosen_language"), langBtn = $("#continueLangBtn"), languageModal = $("#languageModal"),
    theTranslation, theFallbackTranslation;

function SetLanguages(augment) {
    var data = JSON.parse(augment);

    chosenLanguageSelect.empty();
    $.each(data, function (i, thing) {
        chosenLanguageSelect.append("<option name='" + thing + "'>" + thing + "</option>");
    });
    chosenLanguageSelect.find("option:contains('English')").prop("selected", true);
}

chosenLanguageSelect.on("change", function () {
    langBtn.text("Continue with \"" + $(this).val() + "\"");
});

langBtn.on("click", function () {
    window.external.SetLanguage($(chosenLanguageSelect).val());
    languageModal.modal("hide");
});

languageModal.modal("show");

function SetTranslation(translation_json, fallback_json) {
    if (typeof translation_json !== 'undefined') {
        theTranslation = JSON.parse(translation_json);
    }
    if (typeof translation_json !== 'undefined') {
        theFallbackTranslation = fallback_translation = (fallback_json == null ? null : JSON.parse(fallback_json));
    }

    $("[data-translation-key]").each(function () {
        let translationKey = $(this).attr("data-translation-key"), currentTranslation = null;

        if (theTranslation.hasOwnProperty(translationKey)) {
            currentTranslation = theTranslation;
        } else if (theFallbackTranslation != null) {
            if (theFallbackTranslation.hasOwnProperty(translationKey)) {
                currentTranslation = theFallbackTranslation;
            }
        }

        if (currentTranslation != null) {
            let theText = currentTranslation[translationKey];

            if (translationKey == "not_working_modal_opener") {
                theText = theText.replace("{link_start}", "<a data-toggle=\"modal\" data-target=\"#doesntWorkModal\" href=\"#\">");
            } else if (translationKey == "repeat_step_two_suggestion") {
                theText = theText.replace("{link_start}", "<a href=\"#\" style=\"cursor: pointer;\" data-dismiss=\"modal\" onclick=\"setStep(2);\">");
            } else if (translationKey == "expert_setup_chosen_text") {
                $("#expert_setup_card").attr("data-name", theText);
            }

            theText = theText.replace("{link_end}", "</a>");
            theText = theText.replace("{cloud_service}", "<span class='service-name'></span>");

            $(this).html(theText);
        } else {
            if (!$(this).html().length) {
                $(this).html("No translation found :(");
            }
        }
        //
    });

    $(".service-name").text(chosenCloudService);
}


/* END TRANSLATIONS */


var activeStep = 1
    , cloudServiceInstalled = false
    , canProceed = false
    , showProceedTip = false
    , tipTimeout = null
    , nextButton = $("#nextButton")
    , previousButton = $("#previousButton")
    , tipShown = false

    , hasBeenAtStep3 = false
    , actionErrorTimer = null
    , canPopover = false

    , chosenCloudService = ""
    , chosenCloudServiceNum = 0
    , chosenCloudServiceImage

    , stepDivs = [$("#dropbox-install-guide"), $("#onedrive-install-guide"), $("#google-drive-install-guide")]
    , activeTimeout = null
    , hasSetCustomPath = false
    , hasWorkOnedrive = false
    , acc_version;

function SetAccVersionNum(num) {
    acc_version = num;
}

$("#skip_guide_text").on("click", function () {
    window.external.SkipGuide();
});

$("#custom_path_submit").on("click", function (e) {
    e.preventDefault();
    window.external.CheckManualPath($("#manualCheckPath").val());
    $("#fixModalContent").hide();
    $("#loadingModalContent").show();
});

$('#doesntWorkModal').on('hidden.bs.modal', function () {
    $("#pathError").hide();
    $("#pathSuccess").hide();
    $("#loadingModalContent").hide();
    $("#fixModalContent").show();
});

function ManualPathValidated(success) {
    setTimeout(function () {
        if (success) {
            hasSetCustomPath = true;
            canPopover = true;
            canProceed = true;
            cloudServiceInstalled = true;

            $("#loading_container1").stop().fadeOut();
            $("#couldNotFindService").stop().fadeOut();
            $(".strike-out-if-installed").css("text-decoration", "line-through");
            $("#looking_text").stop().fadeOut();
            $("#service_found").stop().fadeIn();

            if (activeStep == 1)
                setNext(true);

            if (canPopover)
                showProceedTip = true;
            //showHelpPopover();

            $("#loadingModalContent").stop().fadeOut("fast", function () {
                $("#pathSuccess").stop().fadeIn("fast");
            });
        } else {
            $("#loadingModalContent").stop().fadeOut("fast", function () {
                $("#pathError").stop().fadeIn("fast");

                activeTimeout = setTimeout(function () {
                    $("#pathError").stop().fadeOut("medium", function () {
                        $("#fixModalContent").stop().fadeIn("medium");
                    });
                }, 8000)
            });
        }
    }, 500);
}

$("#continue_button").on("click", function () {
    window.external.SetupSuccess(chosenCloudService.replace(" ", "").toLocaleLowerCase());
});

function DoneError() {
    canProceed = false;
    tipTimeout = null;
    tipShown = false;
    actionErrorTimer = null;
    hasBeenAtStep3 = false;
    canPopover = false;

    $("#cloud_setup").fadeOut("medium", function () {
        $("#choose_cloud_service").fadeIn("medium");
    });

    stepDivs[0].hide();
    stepDivs[1].hide();
    stepDivs[2].hide();

    chosenCloudService = "";
    chosenCloudServiceNum = 0;

    $(".card-selected").click();
    $("#finishError").modal("show");

    setTimeout(function () {
        $("#finishError").modal("hide");
    }, 10000);
}

$(".card.cloud_service_card").on("click", function () {
    let theThis = $(this), theName = theThis.attr("data-name");
    $(".card").removeClass("card-selected");
    theThis.addClass("card-selected");

    chosenCloudService = theName;
    $(".service-name").text(chosenCloudService);
    $("#pick_btn").prop("disabled", false).html($("#proceed_with_cloudservice_text").html());
    chosenCloudServiceNum = theThis.attr("data-num");
    chosenCloudServiceImage = theThis.attr("data-image");
    cloudServiceInstalled = false;

    if (theName == "OneDrive" && hasWorkOnedrive) {
        $("#multipleOneDrive").show();
        window.external.SetODtype("OneDriveConsumer");
    } else {
        $("#multipleOneDrive").hide();
    }
});

$("#pick_btn").on("click", function () {
    if (chosenCloudServiceNum != 4) {
        if (typeof window.external === "undefined") {
            alert("Setup guide can't communicate with ACC right now. Try again, or maybe close and re-open the software");
            return;
        }

        $(".service-name").text(chosenCloudService);
        previousButton.stop().fadeIn();
        canProceed = false;
        tipTimeout = null;
        tipShown = false;
        actionErrorTimer = null;
        hasBeenAtStep3 = false;
        canPopover = false;
        nextButton.prop("disabled", true).removeClass("blink_me");

        $("#fixModalContent").show();
        $("#loadingModalContent").hide();
        setStep(1);

        $("body").css("overflow-y", "auto");

        if (stepDivs[chosenCloudServiceNum - 1] != null)
            stepDivs[chosenCloudServiceNum - 1].show();
        else alert((chosenCloudServiceNum - 1) + " is null");

        $(".strike-out-if-installed").css("text-decoration", "none");
        $(".strike-out-if-installed-partial").css("text-decoration", "none");

        $("#looking_text").show();
        $("#service_found").hide();

        $("#manualCheckPath").val("");

        let stlgh = $(".shutdown_test_link_gh"), stla = $(".shutdown_test_link_alexa"),
            actionListLink = $("#acc_action_list");

        window.external.CloudServiceChosen(chosenCloudService.replace(" ", "").toLocaleLowerCase());
        //alert("Changing links!");
        switch (chosenCloudService.toLowerCase()) {
            case "google drive":
                actionListLink.attr("href", "https://assistantcomputercontrol.com/?cloud=gd#what-can-it-do");
                stlgh.attr("href", "https://ifttt.com/applets/N5LtdnWq-acc-shut-down-computer-google-drive");
                stla.attr("href", "https://ifttt.com/applets/zjKANwgB-acc-shut-down-computer-google-drive");
                break;
            case "dropbox":
                actionListLink.attr("href", "https://assistantcomputercontrol.com/?cloud=db#what-can-it-do");
                stlgh.attr("href", "https://ifttt.com/applets/W3b7fykE-acc-shut-down-computer-dropbox");
                stla.attr("href", "https://ifttt.com/applets/pCPWA7je-acc-shut-down-computer-dropbox");
                break;
            case "onedrive":
                actionListLink.attr("href", "https://assistantcomputercontrol.com/?cloud=od#what-can-it-do");
                stlgh.attr("href", "https://ifttt.com/applets/SUZHkGLx-acc-shut-down-computer-onedrive");
                stla.attr("href", "https://ifttt.com/applets/JgERNdXS-acc-shut-down-computer-onedrive");
                break;
        }

        $("#install_step_name").text(chosenCloudService);
        $("#step-1-title").html("Install " + chosenCloudService);
        $("#chosenCloudServiceImage").attr("src", chosenCloudServiceImage);

        $("#choose_cloud_service").stop().fadeOut("medium", function () {
            $("#cloud_setup").stop().fadeIn("medium");
        });

        if (hasSetCustomPath) {
            hasSetCustomPath = false;
            window.external.ClearCustomSetPath();
        }
    } else {
        window.external.ExpertChosen();
    }
});

var actionAmounts = 0, actionResultTimeout = null;

function actionWentThrough(status, title, action) {
    if (activeStep != 3) {
        //Done action before step 3, way to go!
        if (!hasBeenAtStep3) {
            if (status == "success") {
                $("#fast_message").show();
            } else {
                $("#too_fast_message").show();
            }
        }

        setStep(3);
    }

    if (status == "success") {
        actionAmounts++;

        if (actionAmounts <= 3) {
            $("#actionProgressBar").attr("data-value", (100 / 3 * actionAmounts)).find(".h2").text(actionAmounts + "/3");
            UpdateActionProgress();
        }

        $("#error_icon, #tryingActions").stop().fadeOut("fast", function () {
            $("#actionDone").stop().fadeIn("medium", function () {

                if (actionResultTimeout != null) {
                    clearTimeout(actionResultTimeout);
                    $("#success_icon").hide();
                }

                $("#success_icon").stop().fadeIn("fast", function () {
                    //$("#status_message").stop().fadeIn().html("<b>\"" + action + "\"</b> action went through!");
                    $("#status_message").stop().fadeIn().html($("#action_went_through").html().replace("{action}", "<b>" + action + "</b>"));
                });

                if (actionAmounts == 3) {
                    $("#continue_button").stop().fadeIn();
                } else {
                    actionResultTimeout = setTimeout(function () {
                        actionResultTimeout = null;

                        $("#actionDone, #success_icon, #error_icon").stop().fadeOut("fast", function () {

                            $("#fast_message").hide();
                            $("#too_fast_message").hide();
                            $("#status_message").hide();

                            $("#status_message").html("");
                            $("#tryingActions").stop().fadeIn();
                        });
                    }, 4000);
                }
            });
        });
    } else if (status == "error") {
        $("#status_message").show();

        $("#timeToTryText").stop().fadeOut("fast", function () {
            $("#continue_button").attr("disabled", true);
            $("#success_icon").stop().fadeOut("fast", function () {
                $("#error_icon").stop().fadeIn();
                $("#continue_button").stop().fadeIn();
            });
        });
    }
    //$("#status_message").text(title);
}

$('[data-toggle="popover"]').popover();
CloudServiceInstalled(cloudServiceInstalled);

nextButton.on("show.bs.popover", function (e) {
    if (!showProceedTip)
        e.preventDefault();
    else
        tipShown = true;
});

function canShowPopover() {
    canPopover = true;

    if (cloudServiceInstalled)
        showHelpPopover();
}

function showHelpPopover() {
    tipTimeout = setTimeout(function () {
        showProceedTip = true;
        nextButton.popover("toggle");
    }, 5000);
}

function CloudServiceInstalled(status, partial) {
    if (partial == null)
        partial = false;

    cloudServiceInstalled = status;
    if (status) {
        //Service installed
        $("#loading_container1").stop().fadeOut();
        $("#couldNotFindService").stop().fadeOut();
        $(".strike-out-if-installed").css("text-decoration", "line-through");
        $("#looking_text").stop().fadeOut();
        $("#service_found").stop().fadeIn();

        if (!partial)
            $(".strike-out-if-installed-partial").css("text-decoration", "line-through");

        if (activeStep == 1 && !partial)
            setNext(true);

        if (canPopover && !partial)
            showProceedTip = true;
        //showHelpPopover();
    } else {
        //Service not installed
        $(".strike-out-if-installed").css("text-decoration", "none");
        $(".strike-out-if-installed-partial").css("text-decoration", "none");

        $("#loading_container1").stop().fadeIn();
        $("#couldNotFindService").stop().fadeIn();
        //$("#step-1-title").html("Install " + chosenCloudService);
        $("#looking_text").stop().fadeIn("fast");
        $("#service_found").stop().fadeOut();

        $("#step-2-btn").attr("disabled", true);
        $("#step-3-btn").attr("disabled", true);
        if (activeStep != 1) {
            setNext(false);
            setStep(1);
        }
    }
}

function setStep(step) {
    activeStep = step;
    if (step == 1) {
        //previousButton.stop().fadeOut();
        nextButton.stop().fadeIn();
        CloudServiceInstalled(cloudServiceInstalled);
        $("body").css("overflow-y", "auto");
    } else if (step == 2) {
        $("body").css("overflow-y", "hidden");

        window.external.SetPath(chosenCloudService.replace(" ", "").toLocaleLowerCase());

        previousButton.stop().fadeIn();
        nextButton.stop().fadeIn();
        setNext(true);
    } else if (step == 3) {
        hasBeenAtStep3 = true;

        previousButton.stop().fadeIn();
        nextButton.stop().fadeOut();
    }

    if (step != 1) {
        $("#step-2-btn").attr("disabled", false);
        $("#step-3-btn").attr("disabled", false);
    }

    $("#step-" + step + "-btn").attr("disabled", false).click();
}

$("#nextButton, #previousButton").hover(function () {
    if (canProceed) {
        $(this).removeClass("blink_me");
    }
    $(this).css("color", "#fff");
    $(this).css("background-color", "#428bca");
    $(this).css("border-color", "#357ebd");
}, function () {
    $(this).css("color", "#212121");
    $(this).css("background-color", "transparent");
    $(this).css("border-color", "transparent");

    if (canProceed && $(this) == nextButton) {
        $(this).addClass("blink_me");
    }
});

previousButton.on("click", function () {
    if (activeStep == 2) {
        $("#step-1-btn").attr("disabled", false).click();
        activeStep = 1;
    } else if (activeStep == 3) {
        $("#step-2-btn").attr("disabled", false).click();
        activeStep = 2;
    } else if (activeStep == 1) {
        //Back to selection
        actionAmounts = 0;
        $("#actionProgressBar").attr("data-value", 0).find(".h2").text("0/3");
        UpdateActionProgress(true);

        $("body").css("overflow", "hidden");
        $("#cloud_setup").stop().fadeOut("medium", function () {
            stepDivs[0].hide();
            stepDivs[1].hide();
            stepDivs[2].hide();

            chosenCloudService = "";
            chosenCloudServiceNum = 0;

            $(".card-selected").click();

            $("#choose_cloud_service").stop().fadeIn("medium");
        });
    }
    setStep(activeStep);
});

function handlePopover() {
    if (tipTimeout != null)
        clearTimeout(tipTimeout);
    showProceedTip = false;
    if (tipShown)
        nextButton.popover("hide");
}

nextButton.on("click", function () {
    handlePopover();

    if (activeStep == 1 && cloudServiceInstalled) {
        $("#step-2-btn").attr("disabled", false).click();
        activeStep = 2;
    } else if (activeStep == 2) {
        $("#step-3-btn").attr("disabled", false).click();
        activeStep = 3;
    }
    setStep(activeStep);
});

function setNext(status) {
    canProceed = status;
    if (status) {
        nextButton.prop("disabled", false).addClass("blink_me");
    } else {
        nextButton.prop("disabled", true).removeClass("blink_me");
    }
}

var navListItems = $('div.setup-panel div a'),
    allWells = $('.setup-content');

navListItems.click(function (e) {
    e.preventDefault();
    var $target = $($(this).attr('href')),
        $item = $(this),
        navNum = $item.attr("data-step-num");

    activeStep = navNum;
    if (activeStep == 3)
        hasBeenAtStep3 = true;

    handlePopover();
    if (activeStep == 1) {
        CloudServiceInstalled(cloudServiceInstalled);
        //previousButton.stop().fadeOut();
        nextButton.stop().fadeIn();
    } else if (activeStep == 3) {
        previousButton.stop().fadeIn();
        nextButton.stop().fadeOut();
    }

    if (!$item.hasClass('disabled')) {
        navListItems.removeClass('btn-primary').addClass('btn-default');
        $item.addClass('btn-primary');
        allWells.hide();
        $target.show();
        $target.find('input:eq(0)').focus();
    }
});

$('div.setup-panel div a.btn-primary').trigger('click');

function HasWorkOneDrive(doesHas) {
    hasWorkOnedrive = doesHas;
}

$(".onedrive_type").on("click", function () {
    $(".onedrive_type").addClass("btn-default").removeClass("btn-primary chosen_onedrive_type");
    $(this).removeClass("btn-default").addClass("btn-primary chosen_onedrive_type");

    window.external.SetODtype($(this).attr("data-onedrive-type"));
});

UpdateActionProgress();
function UpdateActionProgress(reset) {
    $(".progress").each(function () {
        var value = $(this).attr('data-value');
        var left = $(this).find('.progress-left .progress-bar');
        var right = $(this).find('.progress-right .progress-bar');

        if (reset === true) {
            right.css('transform', 'rotate(0deg)')
            left.css('transform', 'rotate(0deg)')
        }

        if (value > 0) {
            if (value <= 50) {
                right.css('transform', 'rotate(' + percentageToDegrees(value) + 'deg)')
            } else {
                right.css('transform', 'rotate(180deg)')
                left.css('transform', 'rotate(' + percentageToDegrees(value - 50) + 'deg)')
            }
        }

    });
}

function percentageToDegrees(percentage) {
    return percentage / 100 * 360
}

// NO SCALING
$(document).keydown(function (event) {
    if (event.ctrlKey == true && (event.which == '61' || event.which == '107' || event.which == '173' || event.which == '109' || event.which == '187' || event.which == '189')) {
        event.preventDefault();
    }
    // 107 Num Key  +
    // 109 Num Key  -
    // 173 Min Key  hyphen/underscor Hey
    // 61 Plus key  +/= key
});

$(window).bind('mousewheel DOMMouseScroll', function (event) {
    if (event.ctrlKey == true) {
        event.preventDefault();
    }
});