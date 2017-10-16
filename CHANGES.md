# 2.0.0
* Add property groups alongside global properties.
* Add PropertyGroupException, which gets thrown when a property group is incorrectly declared (i.e. missing closing brace).
* Make some changed to and deprecate serialization stuff (for now).
	* More error checking in PropertyAttribute(string, Type).
	* Add PropertyAttribute(Type). (This will use the name of field/property when serializing and deserializing).
	* Add AttributeUsage to PropertyAttribute (fields and properties only).
	* Add property converters (NetProperty.Serialization.PropertyConverter) for serialization and deserialization.
	* Ability to deserialize into a provided type.
* Some documentation updates.
* Minor fixes.

# 1.1.0
* Basic serialization and deserialization (Serialization namespace).
* Streams are now accepted when saving and loading.
* More documentation.
* Custom exception (InvalidPropertyException).

# 1.0.0
* All properties are now string values. 
* Added documentation (xml comments and wiki - https://github.com/napen123/NetProperty/wiki).
* Updated the names of the get and set methods (e.g. GetPropertyValue -> GetProperty).
* The constructor no longer takes a file, instead, the Save and Load methods take the parameter.

# 0.0.3
* Basic reading and writing of property files.
