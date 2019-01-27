/* 
XML-RPC.NET library
Copyright (c) 2001-2006, Charles Cook <charlescook@cookcomputing.com>

Permission is hereby granted, free of charge, to any person 
obtaining a copy of this software and associated documentation 
files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, 
publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be 
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.
*/

namespace CookComputing.XmlRpc
{
  using System;
  using System.Collections;
  using System.IO;
  using System.Reflection;
  using System.Text.RegularExpressions;

    using System.Xml;

    public class XmlRpcDocWriter
  {
        
    public static void WriteDoc(XmlWriter wrtr, Type type,
      bool autoDocVersion)
    {
      XmlRpcServiceInfo svcInfo = XmlRpcServiceInfo.CreateServiceInfo(type);

      wrtr.WriteStartElement("html");
      WriteHead(wrtr, svcInfo.Name);
      WriteBody(wrtr, type, autoDocVersion);
      wrtr.WriteEndElement();
    }

    public static void WriteHead(XmlWriter wrtr, string title)
    {
      wrtr.WriteStartElement("head");
      WriteStyle(wrtr);
      WriteTitle(wrtr, title);
      wrtr.WriteEndElement();
    }
    
    public static void WriteFooter(XmlWriter wrtr, Type type,
      bool autoDocVersion)
    {
      wrtr.WriteStartElement("div");
      wrtr.WriteAttributeString("id", "content");

      wrtr.WriteStartElement("h2");
      wrtr.WriteString("&nbsp;");
      wrtr.WriteEndElement();
      

      if (autoDocVersion)
      {
        AssemblyName name1 = type.Assembly.GetName();
        wrtr.WriteString(string.Format("{0} {1}.{2}.{3}&nbsp;&nbsp;&nbsp;", name1.Name, 
          name1.Version.Major, name1.Version.Minor, name1.Version.Build));

        AssemblyName name2 = typeof(XmlRpcServerProtocol).Assembly.GetName();
                wrtr.WriteString(string.Format("{0} {1}.{2}.{3}&nbsp;&nbsp;&nbsp;", name2.Name, 
          name2.Version.Major, name2.Version.Minor, name2.Version.Build));

                wrtr.WriteString(string.Format(".NET CLR {0}.{1}.{2}&nbsp;&nbsp;&nbsp;", 
          Environment.Version.Major,Environment.Version.Minor, 
          Environment.Version.Build));
      }
      wrtr.WriteEndElement();
    }
    
    static void WriteStyle(XmlWriter wrtr)
    {
      wrtr.WriteStartElement("style");
      wrtr.WriteAttributeString("type", "text/css");

      wrtr.WriteString("BODY { color: #000000; background-color: white; font-family: Verdana; margin-left: 0px; margin-top: 0px; }");
      wrtr.WriteString("#content { margin-left: 30px; font-size: .70em; padding-bottom: 2em; }");
      wrtr.WriteString("A:link { color: #336699; font-weight: bold; text-decoration: underline; }");
      wrtr.WriteString("A:visited { color: #6699cc; font-weight: bold; text-decoration: underline; }");
      wrtr.WriteString("A:active { color: #336699; font-weight: bold; text-decoration: underline; }");
      wrtr.WriteString("A:hover { color: cc3300; font-weight: bold; text-decoration: underline; }");
      wrtr.WriteString("P { color: #000000; margin-top: 0px; margin-bottom: 12px; font-family: Verdana; }");
      wrtr.WriteString("pre { background-color: #e5e5cc; padding: 5px; font-family: Courier New; font-size: x-small; margin-top: -5px; border: 1px #f0f0e0 solid; }");
      wrtr.WriteString("td { color: #000000; font-family: Verdana; font-size: .7em; border: solid 1px;  }");
      wrtr.WriteString("h2 { font-size: 1.5em; font-weight: bold; margin-top: 25px; margin-bottom: 10px; border-top: 1px solid #003366; margin-left: -15px; color: #003366; }");
      wrtr.WriteString("h3 { font-size: 1.1em; color: #000000; margin-left: -15px; margin-top: 10px; margin-bottom: 10px; }");
      wrtr.WriteString("ul, ol { margin-top: 10px; margin-left: 20px; }");
      wrtr.WriteString("li { margin-top: 10px; color: #000000; }");
      wrtr.WriteString("font.value { color: darkblue; font: bold; }");
      wrtr.WriteString("font.key { color: darkgreen; font: bold; }");
      wrtr.WriteString(".heading1 { color: #ffffff; font-family: Tahoma; font-size: 26px; font-weight: normal; background-color: #003366; margin-top: 0px; margin-bottom: 0px; margin-left: -30px; padding-top: 10px; padding-bottom: 3px; padding-left: 15px; width: 105%; }");
      wrtr.WriteString(".intro { margin-left: -15px; }");
      wrtr.WriteString("table { border: solid 1px; }");

      wrtr.WriteEndElement();
      
    }

    static void WriteTitle(
      XmlWriter wrtr, 
      string title)
    {
      wrtr.WriteStartElement("title");
      wrtr.WriteString(title);
      wrtr.WriteEndElement();
      
    }

    public static void WriteBody(XmlWriter wrtr, Type type,
      bool autoDocVersion)
    {
      wrtr.WriteStartElement("body");
      

      WriteType(wrtr, type);         
      
      
      WriteFooter(wrtr, type, autoDocVersion);

    // Todo: Check required?
    //wrtr.WriteEndElement("div");

      wrtr.WriteEndElement();
      
    }

    public static void WriteType(
      XmlWriter wrtr, 
      Type type)
    {
      ArrayList structs = new ArrayList();
      
      wrtr.WriteStartElement("div");
      wrtr.WriteAttributeString("id", "content");
      

      XmlRpcServiceInfo svcInfo =
        XmlRpcServiceInfo.CreateServiceInfo(type);

      wrtr.WriteStartElement("p");
      wrtr.WriteAttributeString("class", "heading1");
      wrtr.WriteString(svcInfo.Name);
      wrtr.WriteEndElement();
      wrtr.WriteStartElement("br");
      wrtr.WriteEndElement();
      

      if (svcInfo.Doc != "")
      {
        wrtr.WriteStartElement("p");
        wrtr.WriteAttributeString("class", "intro");
        wrtr.WriteString(svcInfo.Doc);
        wrtr.WriteEndElement();
        
      }
      wrtr.WriteStartElement("p");
      wrtr.WriteAttributeString("class", "intro");
      wrtr.WriteString("The following methods are supported:");
      wrtr.WriteEndElement();
      

      wrtr.WriteStartElement("ul");
      
      foreach (XmlRpcMethodInfo mthdInfo in svcInfo.Methods)
      {
        if (!mthdInfo.IsHidden)
        {
          wrtr.WriteStartElement("li");
          wrtr.WriteStartElement("a");
          wrtr.WriteAttributeString("href", "#"+mthdInfo.XmlRpcName);
          wrtr.WriteString(mthdInfo.XmlRpcName);
          wrtr.WriteEndElement();
          wrtr.WriteEndElement();
          
        }
      }
     
      wrtr.WriteEndElement();
      

      foreach (XmlRpcMethodInfo mthdInfo in svcInfo.Methods)
      {
        if (mthdInfo.IsHidden == false)
          WriteMethod(wrtr, mthdInfo, structs);
      }

      for(int j = 0; j < structs.Count; j++)
      {
        WriteStruct(wrtr, structs[j] as Type, structs);
      }

      wrtr.WriteEndElement();
      
    }

    static void WriteMethod(
      XmlWriter wrtr, 
      XmlRpcMethodInfo mthdInfo,
      ArrayList structs)
    {
      wrtr.WriteStartElement("span");
      
      wrtr.WriteStartElement("h2");
      wrtr.WriteStartElement("a");
      wrtr.WriteAttributeString("name", "#"+mthdInfo.XmlRpcName);
      wrtr.WriteString("method " + mthdInfo.XmlRpcName);
      wrtr.WriteEndElement();
      wrtr.WriteEndElement();
      

      if (mthdInfo.Doc != "")
      {
        wrtr.WriteStartElement("p");
        wrtr.WriteAttributeString("class", "intro");
        wrtr.WriteString(mthdInfo.Doc);
        wrtr.WriteEndElement();
        
      }
      
      wrtr.WriteStartElement("h3");
      wrtr.WriteString("Parameters");
      wrtr.WriteEndElement();
      

      wrtr.WriteStartElement("table");
      wrtr.WriteAttributeString("cellspacing", "0");
      wrtr.WriteAttributeString("cellpadding", "5");
      wrtr.WriteAttributeString("width", "90%");

      if (mthdInfo.Parameters.Length > 0)
      {
        foreach (XmlRpcParameterInfo parInfo in mthdInfo.Parameters)
        {
          wrtr.WriteStartElement("tr");
          wrtr.WriteStartElement("td");
          wrtr.WriteAttributeString("width", "33%");
          WriteType(wrtr, parInfo.Type, parInfo.IsParams, structs);
          wrtr.WriteEndElement();

          wrtr.WriteStartElement("td");
          if (parInfo.Doc == "")
            wrtr.WriteString(parInfo.Name);
          else
          {
            wrtr.WriteString(parInfo.Name);
            wrtr.WriteString(" - ");
            wrtr.WriteString(parInfo.Doc);
          }
          wrtr.WriteEndElement();
          wrtr.WriteEndElement();
        }
      }
      else
      {
        wrtr.WriteStartElement("tr");
        wrtr.WriteStartElement("td");
        wrtr.WriteAttributeString("width", "33%");
        wrtr.WriteString("none");
        wrtr.WriteEndElement();
        wrtr.WriteStartElement("td");
        wrtr.WriteString("&nbsp;");
        wrtr.WriteEndElement();
        wrtr.WriteEndElement();
      }
      wrtr.WriteEndElement();
      
      wrtr.WriteStartElement("h3");
      wrtr.WriteString("Return Value");
      wrtr.WriteEndElement();
      


      wrtr.WriteStartElement("table");
      wrtr.WriteAttributeString("cellspacing", "0");
      wrtr.WriteAttributeString("cellpadding", "5");
      wrtr.WriteAttributeString("width", "90%");

      wrtr.WriteStartElement("tr");

      wrtr.WriteStartElement("td");
      wrtr.WriteAttributeString("width", "33%");
      WriteType(wrtr, mthdInfo.ReturnType, false, structs);
      wrtr.WriteEndElement();

      wrtr.WriteStartElement("td");
      if (mthdInfo.ReturnDoc != "")
        wrtr.WriteString(mthdInfo.ReturnDoc);
      else
        wrtr.WriteString("&nbsp;");
      wrtr.WriteEndElement();
        
      wrtr.WriteEndElement();

      wrtr.WriteEndElement();
      

      wrtr.WriteEndElement();
      
    }

    static void WriteStruct(
      XmlWriter wrtr, 
      Type structType,
      ArrayList structs)
    {
      wrtr.WriteStartElement("span");
      
      wrtr.WriteStartElement("h2");
      wrtr.WriteStartElement("a");
      wrtr.WriteAttributeString("name", "#"+structType.Name);
      
      wrtr.WriteString("struct " + structType.Name);
      wrtr.WriteEndElement();
      wrtr.WriteEndElement();
      
    
      wrtr.WriteStartElement("h3");
      wrtr.WriteString("Members");
      wrtr.WriteEndElement();
          
      wrtr.WriteEndElement();
        
   
      wrtr.WriteStartElement("table");
      wrtr.WriteAttributeString("cellspacing", "0");
      wrtr.WriteAttributeString("cellpadding", "5");
      wrtr.WriteAttributeString("width", "90%");

      MappingAction structAction = MappingAction.Error;
      Attribute structAttr = Attribute.GetCustomAttribute(structType, 
        typeof(XmlRpcMissingMappingAttribute));
      if (structAttr != null && structAttr is XmlRpcMissingMappingAttribute)
      {
        structAction = (structAttr as XmlRpcMissingMappingAttribute).Action;
      }

      MemberInfo[] mis = structType.GetMembers();
      foreach (MemberInfo mi in mis)
      {
        if (mi.MemberType == MemberTypes.Field)
        {
          FieldInfo fi = (FieldInfo)mi;
        
          wrtr.WriteStartElement("tr");

          wrtr.WriteStartElement("td");
          wrtr.WriteAttributeString("width", "33%");
          WriteType(wrtr, fi.FieldType, false, structs);
          wrtr.WriteEndElement();

          wrtr.WriteStartElement("td");
          MappingAction memberAction = structAction;
          Attribute attr = Attribute.GetCustomAttribute(fi, 
            typeof(XmlRpcMissingMappingAttribute));
          if (attr != null && attr is XmlRpcMissingMappingAttribute)
          {
            memberAction = (attr as XmlRpcMissingMappingAttribute).Action;
          }
          string memberName = fi.Name + " ";
          string desc = "";
          Attribute mmbrAttr = Attribute.GetCustomAttribute(fi, 
            typeof(XmlRpcMemberAttribute));
          if (attr != null && mmbrAttr is XmlRpcMemberAttribute)
          {
            if ((mmbrAttr as XmlRpcMemberAttribute).Member != "")
              memberName = (mmbrAttr as XmlRpcMemberAttribute).Member + " ";
            desc = (mmbrAttr as XmlRpcMemberAttribute).Description;
          }
          if (memberAction == MappingAction.Ignore)
            memberName += " (optional) ";
          if (desc != "")
            memberName = memberName + "- " + desc;
          wrtr.WriteString(memberName);
          wrtr.WriteEndElement();
                 
          wrtr.WriteEndElement();
        }
      }
      wrtr.WriteEndElement();
      
   
    }

    static void WriteType(
      XmlWriter wrtr, 
      Type type,
      bool isparams,
      ArrayList structs)
    {
      // TODO: following is hack for case when type is Object
      string xmlRpcType;
      if (!isparams)
      {
        if (type != typeof(Object))
          xmlRpcType = XmlRpcServiceInfo.GetXmlRpcTypeString(type);
        else
          xmlRpcType = "any";
      }
      else
        xmlRpcType = "varargs";
      wrtr.WriteString(xmlRpcType);
      if (xmlRpcType == "struct" && type != typeof(XmlRpcStruct))
      {
        if (!structs.Contains(type))
          structs.Add(type);
        wrtr.WriteString(" ");
        wrtr.WriteStartElement("a");
        wrtr.WriteAttributeString("href", "#"+type.Name);
        wrtr.WriteString(type.Name);
        wrtr.WriteEndElement();
      }
      else if (xmlRpcType == "array" || xmlRpcType == "varargs")
      {
        if (type.GetArrayRank() == 1)  // single dim array
        {
          wrtr.WriteString(" of ");
          Type elemType = type.GetElementType();
          string elemXmlRpcType;
          if (elemType != typeof(Object))
            elemXmlRpcType = XmlRpcServiceInfo.GetXmlRpcTypeString(elemType);
          else
            elemXmlRpcType = "any";
          wrtr.WriteString(elemXmlRpcType);            
          if (elemXmlRpcType == "struct" && elemType != typeof(XmlRpcStruct))
          {
            if (!structs.Contains(elemType))
              structs.Add(elemType);
            wrtr.WriteString(" ");
            wrtr.WriteStartElement("a");
            wrtr.WriteAttributeString("href", "#"+elemType.Name);
            wrtr.WriteString(elemType.Name);
            wrtr.WriteEndElement();
          }
        }
      }
    }
  }
}