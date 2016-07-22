# Keyboard Remote
A keyboard sharing tool through different devices

## Websocket Json Rules
### Forward
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
