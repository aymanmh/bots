# Language Bot
This project uses Azure Bot Service and Microsoft Bot Builder SDK to build a bot that does the following:
- Translate word from english to other language (see commands below).
- Get a word's dictionary definition.
- Get usage examples from around press websites.
- Get the etymology of the word from wiktionary (to be implemented, currently it returns the wiki page entry).
- Get an advanced english vocabulary and it's definition from a pre-constructed db.

###### What is implemented:
- Azure Bot Service (built on an Azure Function).
- Azure Table Storage (the db to get the advanced vocabulary)
- Load libraries from the Language API project to query API and implemented the functionalities listed above.
- Bing Translate API

###### To be implemented:
- Train the bot to use the advanced vocabulary set in conversation with users (currently it doesn't do meaningful conversation, just responding to commands).

###### Usage commands:
 "/ar word(s)" : Translate to Arabic.
 "/jp word(s)" : Translate to Japanese
 "/my word(s)" : Translate to Malay
 "/def word" : Get the dictionary entry of the word.
 "/usage word": Get real word example usage
 "/wiki word" : Get wiktionary entry link.
 "/bigword" : Get an advanced english word along with its dictionary entry and translation.
 
###### Notes:
 - To setup the project; an Azure account is needed (will post the Azure Resource Manager templates soon).
 - You can deploy it locally, with Azure Storage and Azure Bot emulator both installed.
 - Needs some refactoring (dedicated class for managing commands instead of if-statements). 


