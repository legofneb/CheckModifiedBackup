CheckModifiedBackup
==============

This is a C# program made for a very specific purpose.

When you create a backup of a FileShare, you don't expect people to use that backup. Well it happens, and then people want you to fix it. This program finds the differences using LastModifiedDate between files in a backup and files in the current FileShare. It outputs results to a CSV file where Y signals a correct file, and N signifies an incorrect modification to the backup.
