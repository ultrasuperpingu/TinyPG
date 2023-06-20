# TinyPG Tools for VSCode

This package provides syntax highlighting for the TinyPG files in VsCode.

## Installation
Copy the "tinypg" directory in the vscode extension directory:
 - On windows: %USERPROFILE%\.vscode\extensions
 - On Linux: ~/.vscode/extensions
 - On MacOS: ~/.vscode/extensions
 
You can also add a task in the tasks.json file of your projects (in ${workspaceFolder}\.vscode).

If the file does not exists, you can create one with this content:
```json
{
    // See https://go.microsoft.com/fwlink/?LinkId=733558
    // for the documentation about the tasks.json format
    "version": "2.0.0",
    "tasks": [
        {
            "label": "Generate Code TPG",
            "type": "shell",
            "command": "TinyPGCL",
            "args":["${file}"],
            "options": {
                "cwd": "${fileDirname}"
            },
            "problemMatcher": {
                "owner": "tpg",
                "fileLocation": ["absolute"],
                "pattern": {
                  "regexp": "^(.*):(\\d+):(\\d+):\\s+(warning|error):\\s+(.*)$",
                  "file": 1,
                  "line": 2,
                  "column": 3,
                  "severity": 4,
                  "message": 5
                }
            },
            "group":{
                "kind": "build",
                "isDefault": "**/*.tpg"
            }
        }
    ]
}
```

If the file exists, copy paste the task into the "tasks" array in the file.

This will allow you to generate the code by launching the task "Generate Code TPG" directly in VS Code. With need the TinyPGCL executable directory to be in the PATH environment variable.