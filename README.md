# Jal.Router
Just another library to route in/out messages
## Documentation
[Wiki](https://github.com/raulnq/Jal.Router/wiki/10.-Home)
## Road Map
* Multiple exception handling (done)
* Multiple shoutdown watchers (done)
* Store big messages and sagas (done)
* Outgoing message storage (done)
* Documentation
* Unit tests
* Outbound messages should have information of the parent saga
* Outgoing message error handling: be able to setup a multi type destination of the handled message (endpoint, local storage, remote storage, etc)
* Register error resource together with the main one
* Async support
* Allows anonymous handlers
* Incoming message forwarding
* Incoming message error handling rework: be able to setup a multi type destination of the handled message (endpoint, local storage, remote storage, etc)
* Move the json serializer to its own package
* Allow customization at the moment to create resources
* Allow message consumption during scheduled periods of time