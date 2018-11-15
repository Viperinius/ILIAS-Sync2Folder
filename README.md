ILIAS-Sync2Folder
=================
### Summary
Sync2Folder is an unofficial tool to download files from the e-Learning platform ILIAS.
It automatically scans your courses including their content and either shows or downloads the files.
All features can be found at [Features](#features). Part of the features and design are inspired by [ILIASDownloader2](https://github.com/kekru/ILIASDownloader2).

*Note:* The code is currently barely commented and sorted out as this is just a small project.

![pic1](https://github.com/Viperinius/ILIAS-Sync2Folder/raw/master/wiki-pictures/mainwindow-sync.JPG)

### Used libraries
+ [Octokit](https://github.com/octokit/octokit.net) (licensed under MIT License)
+ [Wunder.ClickOnceUninstaller](https://github.com/6wunderkinder/Wunder.ClickOnceUninstaller) (licensed under MIT License)
+ [Dragablz](https://github.com/ButchersBoy/Dragablz) (licensed under MIT License)
+ [MahApps.Metro](https://github.com/MahApps/MahApps.Metro) (licensed under MIT License)

### Installation
Download the vX.X.X.X.zip from the latest release. Extract the zip and run the setup.exe. Your instance should start automatically after installation is finished.

### How to use
When installed, you have to insert a link to your ILIAS login page (if you are not at FH Bielefeld). To change it, switch to *"General Settings"*, paste the login link into the text box and click on *"Check Link"*. If everything went right, your universities name (or some abbreviation) should pop up in the box below. To apply the changes, restart Sync2Folder.

You can now proceed and login via the button in the top right of the window. When the connection is established, a checkmark will appear. To set your destination folder, select the *"Folder Options"* tab and click on the *"..."*. If you want to save courses in folders for each semester, turn on the corresponding options in this tab.

You can now switch to the *"Course Options"* tab and your courses will be shown. Adjust names etc. to your liking and head towards the *"Synchronising"* tab. After pressing the *"Start synchronisation"* button, the programme will get the files from all selected courses.

For more details, visit the [Wiki](https://github.com/Viperinius/ILIAS-Sync2Folder/wiki) page.

### Features
+ download or show files only
+ switch between all files showing and listing new files only
+ choose which courses should be synchronised
+ change the local course folder names to your liking
+ save files either with the same folder structure as in ILIAS (i.e. creating course folders in a root directory) or let the tool create the course folders in subfolders for each semester (editable name template)
+ define, where the downloaded files should be stored
+ progress shown (for each course and in general) and a "new files count"
+ export your settings and import them on another device
+ change the server address to get access to your university's version of ILIAS (Default: FH Bielefeld)
+ optional update notification / receive optional notifications when sync is done
+ open files directly from within the programme
+ if files in ILIAS get updated, the programme marks this file and you can overwrite the local old version (or keep it and disable the files' highlighting)
- UI currently only in English (old UI version with < v2 only in German)

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
+ ~~make the update notification optional / receive optional notifications e.g. when sync is done~~ done.
+ ~~be able to export your settings and import them on another device~~ done.
+ ~~changeable servers (for other universities)~~ done.
+ switchable language (German <-> English)
+ implement an optional automatic synchronisation (automatically check for new files every x minutes)
+ ~~switch from WinForms to WPF in v2 with new UI design~~ done.
+ be able to sort the file list view columns (sort by date / size / title etc)
+ optionally save an old version of a file instead of just overwriting it

### Support
If you have any questions, bugs or just feedback, you have multiple possibilities to make contact:
+ open an issue here at Github
+ visit our Discord server: [Link](http://discord.gg/zxDfVpM)

![](https://img.shields.io/discord/469639729164582912.svg?style=for-the-badge)
