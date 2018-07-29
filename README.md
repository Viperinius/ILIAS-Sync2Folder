ILIAS-Sync2Folder
=================
### Summary
Sync2Folder is a tool to download files from the e-Learning platform ILIAS (currently only FH-Bielefeld).
It automatically scans your courses including their content and either shows or downloads the files.
All features can be found at [Features](#features)
*Note:* The code is currently barely commented and sorted out as this is just a small project.

### Used libraries
+ [Octokit](https://github.com/octokit/octokit.net) (licensed under MIT License)


### Features
+ download or show files only
+ switch between all files showing and listing new files only
+ choose which courses should be synchronised
+ change the local course folder names to your liking
+ save files either with the same folder structure as in ILIAS (i.e. creating course folders in a root directory) or let the tool create the course folders in subfolders for each semester (editable name template)
+ define, where the downloaded files should be stored
+ progress shown (for each course and in general)
+ export your settings and import them on another device
- UI currently only in German

#### Planned Features
+ be able to change the folder structure settings (e.g. turning the "semester structure" on) and move already loaded files to the new structure
+ ~~be able to export your settings and import them on another device~~
+ changeable servers (for other universities)
+ switchable language (German <-> English)
+ implement an optional automatic synchronisation (automatically check for new files every x minutes)

### Support
If you have any questions, bugs or just feedback, you have multiple possibilities to make contact:
1. open an issue here at Github
2. visit our Discord server: [Link](http://discord.gg/zxDfVpM)

![](https://img.shields.io/discord/469639729164582912.svg?style=for-the-badge)
