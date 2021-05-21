# cdc-events
Contains the CDC Events API and Event Generator

# CDC Events API

Is an Azure Function app in C# providing Http Endpoints and methods that defined the API behaviour.

# Event Generator

Is an Azure Logic App calling the Function API through the EAPIM management gateway to apply security, control and management of the API.

# Folders and files

The files in the following repository and solution virtual folders are as follows;

`azure`

Azure service definition templates

 - event-api-template.json : the function application template
 - event-generator-template,json : the logic application template and logic code

`eapim`

The EAPIM service swagger and policy files.

`pipelines`

The Azure pipeline definition files

`src`

The Azure Function application C# Solution code files.