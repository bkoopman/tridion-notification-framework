<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
      <head>
        <title>SDL Tridion Workflow Notification</title>
      </head>
      <body>
        <h1>You have new Workflow notifications</h1>
        <p>
          Dear <xsl:value-of select="/WorkflowInfo/UserData/Description" />,
        </p>
        You have the following new items in your Assignment List:
        <ul>
          <xsl:for-each select="/WorkflowInfo/ArrayOfWorkItemData/WorkItemData">
            <li>
              <a href="someURL">Some shit</a>
            </li>
          </xsl:for-each>
        </ul>
        <p>
          Just beat it<br /><br />
          The MVP Team
        </p>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>