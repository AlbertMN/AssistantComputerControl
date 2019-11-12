# Contributing to AssistantComputerControl

Thanks for your interest in helping the project!

The following is a set of guidelines for contributing to the AssistantComputerControl project. These are mostly guidelines, not rules.

#### Table Of Contents

[How Can I Contribute?](#how-can-i-contribute)
  * [Reporting Bugs](#reporting-bugs)
  * [Suggestings](#suggestions)
  * [Pull Requests (code contribution)](#pull-requests-code-contribution)

Additional
  * [Code Styleguide](#code-styleguide)

## Reporting bugs
Found a bug? Let's get it fixed! For us to get the issue resolved as quick as possible, we need all the information you can provide us.

:heavy_exclamation_mark: **Before you open an issue** here on the project page, please take the time to check [the issue list](https://github.com/AlbertMN/AssistantComputerControl/issues?utf8=%E2%9C%93&q=is%3Aissue) to see if the problem you're experiencing has already been reported. If your issue is somewhat like another issue, but not completely the same, include a link to the issue in the issue you create.

Has the issue you're having has not been reported yet? Then [open a new issue](https://github.com/AlbertMN/AssistantComputerControl/issues/new) and describe the issue in as much details as you can.

**General details to include in your issue report;**
- When does the issue occour? _(how to make it happen)_
- How often does it happen?
- Which version of the software are you on?
- Did this happen in the last update as well?

**You should also attach any ACC log files**, located on your computer at; `Documents/AssistantComputerControl/`. This folder can contain a `log.txt` and `error_log.txt` file - attach both if they are present.

Once you feel like you have described your issue as well as possible, go ahead and create the issue, and wait for a reponse. Issues are often responded to within one day and resolved in a matter of days as well.
How fast the issue is resolved also heavily depends on your cooperation. Often times the developer(s) can't replicate the issue, and in this case you will be asked to test new versions of the software to see if the problem has been resolved.

## Suggestions
All suggestions are welcome! Most of what the software can has been added as a result of user suggestions. We prefer to keep suggestions in the [Discord server](https://discord.gg/B9YGPNF), in the `#suggestions` channel.

## Pull Requests (code contribution)
Want to contribute to the project by coding a new feature, fixing a bug or generally just improve the project? Your help is very welcome!

Seeing the software has tens of thousands of users using it every day, the accepted code-contributions however _does not_ include;
- Major rewrites of entire files
  - _To keep the project manageable, secure and reliable, development of large scale functionality that isn't a new action won't be accepted_
- Changes to the way core parts of the software works
  - _Fx the way we communicate with the cloud services or IFTTT_
  
All other contributions are very welcome! Please refer to the [Code Styleguide](#code-styleguide) before writing code for the project, to ensure your code matches the style of the project.

____

## Code Styleguide
Before making code contributions to the project, there are a few "rules" regarding the way the code looks.

**General info**
- Our brace indentation standard is [K&R](https://en.wikipedia.org/wiki/Indentation_style#K&R_style) _(braces on same line as the statement)_
- We use `tabs` - not spaces.
- The project is a Visual Studio project running on .NET Framework 4.6
- The primary language is C#, only other programming language used is JavaScript and a bit of 

:white_check_mark: **ACC coding style;**
```
if (condition) {
  //code
} else {

}

class x {
  //
}
var variable1 = "",
  variable2 = "",
  variable3 = "";
```

:x: **"Wrong" coding style**

_Not that this codings tyle is "wrong", it's just not the style used in this particular project - it's a matter of taste._
```
if (condition)
{
  //code
}
else
{

}

class x
{
  //
}
var variable1 = "", variable2 = "", variable3 = "";
```

If you have any questions about something else regarding the coding style, all you have to do is take a look at the code, and do like it's done there. 

