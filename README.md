# Input Remote
Control your PC with your smartphone,
a HID input sharing tool.

## Install
**Windows**

Simply download the [current alpha build v0.1.0](https://github.com/AnthonyFu117/input-remote/releases/tag/v0.1.0-alpha), decompress and execute `InputRemote.Client.Receiver.exe`. You may also need to install [.Net Framework 4.0](https://www.microsoft.com/en-us/download/details.aspx?id=17851).

**Linux / Mac OS X**

We only support Windows for now. The supporting for Linux / Mac OS X may be met in future releases.

## Usage
#### PC
1. Connect your smartphone and your PC into a **same** LAN network.
1. Check your Firewall rules had open income port `80` and `81`
1. Get your PC's LAN IP address command `ipconfig`
1. Execute `InputRemote.Client.Receiver.exe`
1. Check the `Enable` context menu of tray icon

#### Phone
1. Open your web browser and key in the IP address you got from your PC
1. You may see the tray icon of PC turn `Green` and `Sender Online` displayed on your phone browser.
1. Now you can access your PC by taping and sliding on your smartphone screen. Enjoy it!


## Develop

#### ToDo
[Todo](doc/todo.md)

#### Doc
[Communication](doc/communication.md)

[Server](doc/server.md)

[Client](doc/client.md)


## License
MIT
