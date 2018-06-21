# 2.1.0
* Add TryLoad for not throwing an exception when there are erroneous properties. 
* Simplify GetProperty and indexer (no longer using Linq).

# 2.0.1
* Add various different targets:
	* .NET Framework 4.5 - 4.7.1
	* .NET Core 2.0
	* .NET Standard 2.0
* Fixed a bug related to saving to a nonexistant file.

# 2.0.0
* Add new constructors to PropertyFile:
    * PropertyFile(int) to specify initial property capacity.
	* Updated the file and stream constructors to add the ability to specify an encoding
	  and whether or not to treat empty values as null.
* Make some minor changes to and deprecate serialization stuff (for now).
	* More error checking in PropertyAttribute(string, Type).
	* Add PropertyAttribute(Type). (This will use the name of field/property when serializing and deserializing).
	* Add AttributeUsage to PropertyAttribute (fields and properties only).
	* Add property converters (NetProperty.Serialization.PropertyConverter) for serialization and deserialization.
	* Ability to deserialize into a provided type.
* Make SetProperty and GetProperty return error-values instead of throwing exceptions.
* Add the ability to treat empty values as null when loading from files and streams.
* Saving properties with null values will result in them having an empty assignment.
	- For example, if property "name" has null as its value, the property's output in a file would look like "name ~"
* Fix memory leaks in Load(string, Encoding) and Save(string, Encoding).
* Documentation updates.
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
