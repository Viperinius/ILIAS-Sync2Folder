ILIAS-Sync2Folder
=================
### Summary
Sync2Folder is a tool to download files from the e-Learning platform ILIAS.
It automatically scans your courses including their content and either shows or downloads the files.
All features can be found at [Features](#features)

*Note:* The code is currently barely commented and sorted out as this is just a small project.

### Used libraries
+ [Octokit](https://github.com/octokit/octokit.net) (licensed under MIT License)
+ [Wunder.ClickOnceUninstaller](https://github.com/6wunderkinder/Wunder.ClickOnceUninstaller) (licensed under MIT License)

### How to use
*Coming shortly in wiki section.*

### Features
+ download or show files only
+ switch between all files showing and listing new files only
+ choose which courses should be synchronised
+ change the local course folder names to your liking
+ save files either with the same folder structure as in ILIAS (i.e. creating course folders in a root directory) or let the tool create the course folders in subfolders for each semester (editable name template)
+ define, where the downloaded files should be stored
+ progress shown (for each course and in general)
+ export your settings and import them on another device
+ change the server address to get access to your university's version of ILIAS (Default: FH Bielefeld)
- UI currently only in German (new UI version only in English)

#### Note
As I'm currently not able to test Sync2Folder with other universities than FH Bielefeld, it is not sure whether or not features like the structured folders will work as they heavily depend on the course naming.
Example of a naming version that works:	
>ELM-4.2-SST, Signal- und Systemtheorie, Battermann, SS2018

so like 

>...-SemesterNum.CourseNum, ..., ..., SemesterYear

#### Note 2
Regarding the server change:
The link check allows aside from the login page the web service page, too. Due to some connection problems, this is currently not working.
So just stick to use the login link to set your ILIAS server for now.

#### Planned Features
+ be able to change the folder structure settings (e.g. turning the "semester structure" on) and move already loaded files to the new structure
+ make the update notification optional / receive optional notifications e.g. when sync is done
+ ~~be able to export your settings and import them on another device~~ done.
+ ~~changeable servers (for other universities)~~ done.
+ switchable language (German <-> English)
+ implement an optional automatic synchronisation (automatically check for new files every x minutes)
+ switch from WinForms to WPF in v2 with new UI design

### Support
If you have any questions, bugs or just feedback, you have multiple possibilities to make contact:
1. open an issue here at Github
2. visit our Discord server: [Link](http://discord.gg/zxDfVpM)

![](https://img.shields.io/discord/469639729164582912.svg?style=for-the-badge)
