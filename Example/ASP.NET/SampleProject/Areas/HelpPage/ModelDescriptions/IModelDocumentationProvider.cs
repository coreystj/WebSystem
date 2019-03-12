using System;
using System.Reflection;

namespace WebSystem.Example.ASP.NET.SampleProject.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}