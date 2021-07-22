Notes: On Logic App script round trip from the Azure Logic App Code view to Repository.

In Azure development subscription
In the logic app.
Select the logic app code view tab
Select all ths Json except the starting and ending braces { }

In Visual Studio
Open the azure/event-generator-template.json file
Apste the code into the resources:properties 'definition' and 'parameters' properties as a replacement. This is the entire APP from Azure in iots ARM tempalte location.

Open a powershell window
Run the src/event-generator-template.ps1 script

Back in Visual Studio re-vist the event-generator-template.json window which should now be a single line.
It may complain about line endings just click yes.

Then Control-E,D (Menu Edit/Advanced/Format Document) to reformat the file.

The paramter hard-codings have now been replaced with the correct ARM Tempalte parameter and variable values

Modify the PS script to add new mappings as needed.

