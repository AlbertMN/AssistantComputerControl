# AssistantComputerControl
Control your computer using your **Google Home**, **Amazon Alexa** and **Microsoft Cortana** assistants!

This is a simple but powerful tool that allows you to perform tasks on your Windows computer like;
### Supported computer actions
* Shutdown [[GH](https://ifttt.com/applets/W3b7fykE-acc-shut-down-computer)][[Alexa](https://ifttt.com/applets/pCPWA7je-acc-shut-down-computer)]
* Open a file [[GH](https://ifttt.com/applets/Pny8DKBL-acc-open-file-example)][[Alexa](https://ifttt.com/applets/EsP6zWpe-acc-open-file-example)]
* Restart [[GH](https://ifttt.com/applets/nsvdVxZ9-acc-restart-computer)][[Alexa](https://ifttt.com/applets/kkwxdE9T-acc-restart-computer)]
* Sleep / Hibernate [[GH](https://ifttt.com/applets/mEbJCP8F-acc-sleep-computer)][[Alexa](https://ifttt.com/applets/Kagf93wH-acc-sleep-computer)]
* Lock [[GH](https://ifttt.com/applets/epbqzfCa-acc-lock-computer)][[Alexa](https://ifttt.com/applets/Gv9Ts8ip-acc-lock-computer)]
* Logout [[GH](https://ifttt.com/applets/TXr8DLHR-acc-log-out-of-your-computer)][[Alexa](https://ifttt.com/applets/QYJyc8HT-acc-log-out-of-your-computer)]
* Set volume to `percent` [[GH](https://ifttt.com/applets/scgDySn4-acc-set-computer-volume-percent)]
* Mute / unmute [[GH](https://ifttt.com/applets/ju6VYneQ-acc-toggle-mute-on-computer)][[Alexa](https://ifttt.com/applets/UZT7hv9a-acc-toggle-mute-on-computer)]
* Music control:
  * Previous song [[GH](https://ifttt.com/applets/qCJL4d9i-acc-play-previous-song-on-computer)][[Alexa](https://ifttt.com/applets/FACuUJKj-acc-play-previous-song-on-computer)]
  * Pause / play [[GH](https://ifttt.com/applets/Wt2uXyAi-acc-play-pause-music-on-computer)][[Alexa](https://ifttt.com/applets/G7ZpsDWM-acc-play-pause-music-on-computer)]
  * Next song [[GH](https://ifttt.com/applets/urhc2Ug8-acc-play-next-song-on-computer)][[Alexa](https://ifttt.com/applets/hq7nxkf3-acc-play-next-song-on-computer)]
* _more to come_

**Join our Discord server:** https://discord.gg/ukkUu26

**Follow me on Twitter for updates:** https://twitter.com/ACC_HomeAlexa

This software only works on Windows and has yet only been tested on Windows 10.

## Setup
The setup is _very_ simple. It only takes a few minutes to set up!

### Step 1: Dependencies
All you need is an [IFTTT](https://ifttt.com/) and a [Dropbox](https://www.dropbox.com/) account plus the Dropbox software installed on your computer.
_Note: for AssistantComputerControl to work Dropbox will need to be running on your PC at all times_

### Step 2: Install
When you have both, you can simply enable _[this applet](https://ifttt.com/applets/xk7JPtWu-acc-install-assistantcomputercontrol)_ and say "OK Google, Assistant Computer Control download" or "OK Google, Install Assistant Computer Control" and the software will be downloaded to your computer!

### Step 3: Setup done!
The following step will create a folder in your Dropbox called "AssistantComputerControl" with a file called `OPEN_ME.exe` - to finish the setup you simply have to open this file and you're just one step away from being done! Next thing we want to do is making the ACC software start automatically everytime your computer starts. To do this just create a shortcut of the `OPEN_THIS.exe` and put it in the Windows Startup folder, which you can find by pressing `Win + R` and typing `shell:startup`; place the shortcut in this folder and you're done!

### Step 4: Adding actions
To do actions on your computer, go to [this IFTTT profile](https://ifttt.com/makers/assistantcomputercontrol) and enable the applets you want!

---

If you want more freedom and set all of this up yourself you can do that! [Here](https://github.com/AlbertMN/AssistantComputerControl/wiki/Manual-setup) is a guide on how to manually set up the software (also fairly simple), which allows you to decide where the software is on your computer and much more.

For more nerdy technical info you can go to the [Wiki](https://github.com/AlbertMN/AssistantComputerControl/wiki) and take a look at the other articles as well.

So as you can see no coding is required! But if you're up for it and want to add your own features to the project it's easy! The entire project is made made in C# and the code is opensource and quite simple, so feel free to use the code in your own fork or even better; submit pull requests!

### List of supported actions and parameters
Visit the ["Actions"](https://github.com/AlbertMN/HomeComputerControl/wiki/Actions) article in the Wiki.

Set any of these as "Content" in the creation of the Dropbox file (IFTTT) and it will work!
