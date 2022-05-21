# F1-Telemetry-App
This app is designed to take advantage of the F1 Games' UDP Telemetry option, enabling an external device on your computer to display any information that the user wishes to see.

## Firebase Realtime Database Emulator setup

### 1. Create a credentials.txt file

1. Create a credentials.txt file in the root directory of the F1-Telemetry-App project.
2. In the credentials.txt file, insert the following:
```
emulator host	<host address & port>
secret	<firebase secret key>
```

where *emulator host* is the full address to the firebase realtime database emulator, and *secret* is the Database secret that you can get from the https://console.firebase.google.com/ project settings.
Note: There is a tab between the key and the value

### 2. Download the Firebase emulator

1. Go to https://firebase.google.com/docs/cli#install-cli-windows to download the firebase CLI emulator.
2. Add the firebase CLI .exe file path to the *PATH* environment variable (e.g. if the firebase.exe is in C:\this\dir\here\firebase.exe, add C:\this\dir\here\ to the *PATH* env var)


### 3. Create the firebase emulator

1. Open a cmd prompt
2. cd into the F1-Telemetry-App root directory
3. Call the firebase emulator .exe (I renamed the file firebase.exe so I only need to call *firebase*) using the following line:
	`firebase init FirebaseEmulator`
	Note: specifically use *FirebaseEmulator* as the directory because this has been added to the .gitignore file
4. Go through the basic setup (You don't really need any of the extra setup that it suggests)

### 4. Start the firebase emulator

5. cd into the *FirebaseEmulator* dir
6. Type the following: `firebase emulators:start`
