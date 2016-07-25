# Communication
Websocket with Json

## Websocket Json Protocol
#### Template
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

#### Mouse Data
```javascript
// TODO
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
