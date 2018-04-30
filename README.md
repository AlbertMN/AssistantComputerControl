# AssistantComputerControl
Control your computer using your **Google Home**, **Amazon Alexa** and **Microsoft Cortana** assistants!

This is a simple but powerful tool that allows you to perform tasks on your Windows computer like;
### Supported computer actions
* Shutdown
* Open a file
* Restart
* Sleep / Hibernate
* Lock
* Logout
* Set volume to `percent`
* Mute / unmute
* Music control:
  * Previous song
  * Pause / play
  * Next song
* _more to come_

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
