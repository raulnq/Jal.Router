# Jal.Router
Just another library to route in/out messages
## Documentation
[Wiki](https://github.com/raulnq/Jal.Router/wiki/10.-Home)
## Road Map
* Multiple exception handling (done)
* Multiple shoutdown watchers (done)
* Store big messages and sagas (done)
* Store data of the host (done)
* Message compression and encryption
* Nuget reference cleanup
* Outgoing message storage (done)
* Documentation
* Unit tests
* Outgoing messages should have information of the parent saga (done)
* Outgoing message error handling: be able to setup a multi type destination of the handled message (endpoint, local storage, remote storage, etc)(done)
* Register error resource together with the main one
* Async support (done)
* Allows anonymous handlers
* Incoming message forwarding (done)
* Incoming message error handling rework: be able to setup a multi type destination of the handled message (endpoint, local storage, remote storage, etc) (done)
* Move the json serializer to its own package (done)
* Allow customization at the moment to create resources (done)
* Allow message consumption during scheduled periods of time
* Second level retry logic (done)
* Send a message directly to the pipeline.
* Bug: the message inside of a saga is having the default status on the storage (done)