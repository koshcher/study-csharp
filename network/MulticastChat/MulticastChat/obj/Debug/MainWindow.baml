   M S B A M L   `   `   `  ????H  DMulticastChat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:MulticastChatMulticastChat  5   	       MulticastChat.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:MulticastChat     5   	     ? Title$  
MainWindow??5   	   $	??450??6   $	??800??6)   .??5	      ?  ? RowDefinitions 5
      ?? 5      $
??auto ?6   ?? 5      $??* ?6   .+?5   
   ?? $??0??6   $	??Top=?6"   .+?5      ??  ?? Orientation$ 
Horizontal=?6   .+?5      ?? $??
Remote IP:??6   6/   ?? 5      -   5         ??Name$ txtRemoteIP??5      $??239.0.0.222??6/   ?? 5      $??Port:??6   ?? 5      -   5        $ txtPort??5      $
??4000??6+   ?? 5      $??Message:??6   ?? 5      -   5        $ 
txtMessage??5      $	??250??6.   $?? ??6:   ?? 5      $??
User Name:??6   ?? 5      -   5        $ userNameText??5      $	??216??60   $?? ??6<   5      ?? 5      $ 
Horizontal=?6   .+?5      ?? -   5         ??Name$ btnStart??5      $	??200??6B   .??6N   Start Client6\   ?? 5      -   5        $ btnSend??5      $	??200??6@   .??6L   Send message6Z   5      5      ? 5      $??1??6    ? ColumnDefinitions 5   
   ?? 5       ?? 5!      .+?5#   
   ?? $ Vertical=?6   $??0??6,   .+?5$      ?? $
??Log:??6   ?? 5%      -   5        $ txtLog??5%      $??Visible=?6$   5&      ?? 5'   
   -   5        $ usersLog??5'      $??1??6%   $??Visible=?65   5(   	   5)      5*      