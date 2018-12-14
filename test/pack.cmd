
@set pundit=..\src\Pundit\bin\Debug\net461\pundit 

%pundit% pack pundit-MyApplication.Common.xml
%pundit% pack pundit-MyApplication.Test.xml
%pundit% pack pundit-MyApplication.Tool.xml
%pundit% pack pundit-MyApplication.ConfigTest.xml
%pundit% pack pundit-MultiFramework.xml

move MyApplication.*.nupkg repo