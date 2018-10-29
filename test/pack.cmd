
@set pundit=..\src\Pundit\bin\Debug\pundit 

%pundit% pack pundit-MyApplication.Common.xml
%pundit% pack pundit-MyApplication.Test.xml
%pundit% pack pundit-MyApplication.Tool.xml

copy MyApplication.*.pundit C:\Users\Enrique\.pundit\remote
del MyApplication.*.pundit