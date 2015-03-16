# Status #
Draft for discussion

# Introduction #

There are various parts of the system that create or consume application data. Specifically, the service, the user interface, and the notifiers all have expectations about what the data should look like. This design attempts to provide a central reference for all these sub-systems


# Details #

## User Application data ##
We have decided to use application data on the User objects to manage notifications. The Tridion Application data API specifies three fields:

  1. Application Id
> > This allows data to be identified as being "owned" by a given piece of software. For example, the Tridion CME uses application data on users, for which the application id is "cme:UserPreferences". For our framework, we will use an application Id of "code.google.com/p/tridion-notification-framework". (The /p/ ensures that if a curious person should paste this into their browser's address bar, they will end up at the right place)


> 2) Data
The data format is not specified by Tridion, which simply stores raw bytes for you, and serves them back on demand. For our framework, the data will be an XML document, the format of which is described below.

> 3) Type
This is simply a text field which you can use to help your software understand how to interpret the raw bytes of the Data field. As our design already dictates an XML document, which may contain data relevant to various kinds of notifiers, we will use a default value of "xml".

## Data format ##
### Encoding ###
The bytes stored/retrieved via the Data property will be an XML document encoded as UTF8
### XML ###
The XML document will have a root element called "NotificationFramework". Note: Namespaces, and/or more elaborate names, are redundant, because the application ID provides sufficient isolation.

There are several values that control/configure the way the service invokes notifiers. Beyond that, each notifier will have its own specific fields that provide the data which will be used by the notifier itself.

Each notifier will be represented by a Notifier element. Each Notifier element will have a "type" attribute which indicates which type of notifier can consume this data. For each Notifier element, the service must guarantee that it invokes each notifier that might be able to consume the Notifier element. If a mechanism is implemented whereby a notifier can indicate the types of Notifier element it can consume, the service may use this mechanism to reduce the number of invocations.

The values which control notification will begin with "notification_". These attributes are listed below under "Notification Attributes"._

Depending on the type of Notifier element, other elements may be present, so for example, an email notifier will require an email address.

#### Notification Attributes ####
**notification\_frequency - this allows you to specify a notification frequency as an integer followed by either H, M, or D to indicate Hours, Minutes or Days** notification\_last\_send - this field is used by the service to store the last time that the notification was executed. Any string which can be successfully parsed by .NET's DateTime class is acceptable, but the ISO 8601 format is preferred.

#### Sample Xml ####
```xml

<NotificationFramework>

<Notifier type="WorkflowEmailNotifier"
notification_frequency="3D"
notification_last_send="2012-10-07T23:13Z">
<EmailAddress>punter@outfit.org

Unknown end tag for &lt;/EmailAddress&gt;





Unknown end tag for &lt;/Notifier&gt;



<Notifier type="WorkflowTwitterNotifier"
notification_frequency="3D"
notification_last_send="2012-10-07T23:13Z">
<TwitterName>TridionLovingHackyGeek

Unknown end tag for &lt;/TwitterName&gt;





Unknown end tag for &lt;/Notifier&gt;





Unknown end tag for &lt;/NotificationFramework&gt;


```