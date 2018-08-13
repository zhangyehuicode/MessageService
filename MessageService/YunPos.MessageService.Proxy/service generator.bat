set url=http://localhost:8733/MessageService/mex
set ctype=System.Collections.Generic.IList`1
set namespace=YunPos.MessageService.Proxy
set svcUtilPath="C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools\SvcUtil.exe"

%svcUtilPath% /s /ct:%ctype% /out:MessageService.cs /namespace:*,%namespace% %url% /syncOnly