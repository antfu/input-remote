# Keyboard Remote
A keyboard sharing tool through different devices

## Implement

#### Server
*Python3.5* with
> Tornado

#### Client/Windows
C# with WinForm
> [websocket-sharp](https://github.com/sta/websocket-sharp)

#### Communication
Websocket with Json

## URL Rules
#### Sender
> Page: http://localhost/sender?c=IhjehI&a=n95fh7

> Websocket: http://localhost/ws?c=IhjehI&a=n95fh7&t=sender

`c` for `channel`, `a` for `authenticate`, `t` for `type`

#### Receiver
> Page: http://localhost/receiver?c=IhjehI&a=n95fh7

> Websocket: http://localhost/ws?c=IhjehI&a=n95fh7&t=receiver


## Websocket Json Rules
### Rule
#### Base
```javascript
{
  action: [system|key|mouse|command],
  subaction: [peerstate|keyaction|mouseaction|...]
  data: [data]
}
```
#### System Data
```javascript
{
  state: bool
  state_msg: string
}
```

#### Key Data
```javascript
{
  keyaction: [keyup|keydown],
  key: "s",
  keycode: 83,
  is_shift_down: bool,
  is_ctrl_down: bool,
  is_alt_down: bool,
}
```

### Example
#### Online State
```json
{  
   "action":"system",
   "subaction":"peerstate",
   "data":{  
      "state":true
   }
}
```
#### Key H Up
```json
{  
   "action":"key",
   "subaction":"keyup",
   "data":{  
      "keyaction":"keyup",
      "key":"H",
      "keycode":"72",
      "is_shift_down":false,
      "is_ctrl_down":false,
      "is_alt_down":false
   }
}
```


## State
|WS Connection|Peer Online|State|
|---|---|---|
|false|false|Disconnect|
|true|false|Waiting|
|true|true|Online|
