Notes: On Logic App script round trip from the Azure Logic App Code view to this repository.

In Azure development subscription
In the logic app designer.
Select the logic app code view tab
Select all the Json except the starting and ending braces { }

In Visual Studio
Open the azure/event-generator-template.json file
Paste the code into the resources:properties 'definition' and 'parameters' properties as a replacement. 
This is the entire APP from Azure placed in its ARM template location.

Open a powershell window
Run the src/event-generator-template.ps1 script.
this will repalce the hard coded variables in the logic code with ARM Template paramter and variable selectors, and set specific values.

NOTE: Modify the PS script to add new mappings as needed.

Back in Visual Studio re-vist the opened event-generator-template.json window which should now be a single line.
It may complain about line endings just click yes.

Then Control-E,D (Menu Edit/Advanced/Format Document) to reformat the file.

Then save it. Now you can perform a Compare with Unmodified to review the changes.

A log analytics workspace was added to enable custom output; a json body template is required for the request from the Logic App. 
The unescaping when converting back to json caused problems so the template string is escaped - note that if further logging steps are added
they will need to be treated in the same way

GOTCHAs

This was noticed from time to time, in dealing with the Azure Logic app designer.

The left hand side of "equals" comparisons in Validate-* sections sometimes goes missing from multiple sections for no reason. Leading to run time failures.

Search for the existence of '"equals, [ ""' (between '') to see if this has occured.
