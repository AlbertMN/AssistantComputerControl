# HomeComputerControl
Control your computer with Google Home!

This simple program lets you tell your Google Home to do basic tasks on your computer like;
### Supported computer actions
- Shutdown
- Restart
- Sleep / Hibernate
- Lock
- Logout
- _more to come_

This software is Windows, only made for, and tested on, Windows 10 so far.

No coding is required - you can simply grab the `HomeComputerControl.exe` file and follow the setup-instructions below.
If you want to add your own features to the project it's easy! The entire project is made made in C#. The code is opensource and quite simple, so feel free to use the code in your own fork or even better; submit pull requests!

## Setup
All this program does is tell the computer what to do from reading a `.txt` file and execute the action that the file tells it to. Everything with the Google Home is handled [IFTTT](https://ifttt.com/) & [Dropbox](https://www.dropbox.com/).
If you're familiare with IFTTT you can probably skip the first three steps.

In this setup guide we will setup the shutdown action.

### Step 1
Register an IFTTT and Dropbox account if you haven't already

**Requirements;**
- An [IFTTT](https://ifttt.com/) account
- A [Dropbox](https://www.dropbox.com/) account


### Step 2
_Creating the applet "this"_
- Go to https://ifttt.com/create to create a new applet
- Press "+this" and type "Google Assistant" in the search field
- Click the Google Assistant box and choose "Say a simple phrase"

### Step 3
_Creating the trigger_
Here you have to define what Google will react to. The first three input fields are for the voice-commands that will activate the shutdown sequence.
Below is an example. You can write _whatever_ you want.
![Trigger example](http://i.albe.pw/eIFbS.PNG)

The next field defines what you want the Google Assistant to respond, could be "Okay, turning off your computer" or "Alright boss" - anything.

When you're finished, press "Create Trigger"

### Step 4
Now we've defined what will trigger the computer-shutdown, next press "+that".
- Search for "Dropbox"
- Select "Create a text file"

Once again we have a few input fields that we need to fill out, but now it's important that we put the _right_ content into these inputs.

The first field, "File name" should be `computerAction`. The "Content" field below needs to be `shutdown`.
Now, the last field, "Dropbox folder path" can be anything you like. This is just the folder in your Dropbox the file will be created in. In this setup guide we're gonna go for "IFTTT/".

!["Create a text file" through Dropbox on IFTTT](http://i.albe.pw/cglcw.PNG)

When you're done, press "Create action"

### Step 5
The next input that pops up is just the title / description for your applet, it can be anything you like. To finish the applet press "Finish".
Now we're done on the IFTTT-side. To test your applet you can say "Hey Google -" and then the trigger you entered in _Step 3_. For me it's "Hey Google, shut down my computer" and Google will respond "Turning off your computer". This should create a file called `computerAction.txt`, containing `shutdown`, in the folder you specified.

### Step 6
Grab the latest `HomeComputerControl.exe` from [Releases](https://github.com/AlbertMN/HomeComputerControl/releases) and place the file in the same Dropbox-folder that the `computerAction.txt` file is being generated in. As this is a new ".exe" file that Windows hasn't seen before, your anti-virus will most likely be protesting against you downloading the file, simply ignore the warnings and proceed with the download if it lets you. _Note if **you** are having doubts about the file the entire source-code can be found [here](https://github.com/AlbertMN/HomeComputerControl/blob/master/HomeComputerControl/Program.cs)_

![Dropbox folder example (how it should look)](http://i.albe.pw/DWs0q.PNG)

### Step 7
If you want this software to work every time you start your computer (which you most likely do), you need to add a shortcut of the `HomeComputerControl.exe` in the Windows Startup-folder:
- Press "Win + R" keys
- Type `shell:startup` and press `OK`
- Place the shortcut here _("[How to create a shortcut on Windows](http://www.thewindowsclub.com/create-desktop-shortcut-windows-10)")_

### Done!
Now open the `HomeComputerControl.exe` file and you're good to go!
Try tell your (Google) assistant to shut down your computer now, and it will do it!


## How to impliment the other actions
The setup just showed how to shutdown your computer, not restart, sleep, logout etc. These setups are almost _exactly_ the same as shutdown. All you have to do is type any of the following lines instead of `shutdown` in the "Content" field _(Step 4)_;

### List of supported actions
- `shutdown`
- `restart`
- `sleep`
- `logout`
- `lock`

Set any of these as "Content" in the creation of the Dropbox file (IFTTT) and it will work!
