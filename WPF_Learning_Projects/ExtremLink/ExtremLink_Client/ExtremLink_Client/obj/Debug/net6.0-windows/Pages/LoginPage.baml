   M S B A M L   `   `   `  ����L  HExtremLink_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:ExtremLink_Client.PagesExtremLink_Client.Pages  5      Q-clr-namespace:ExtremLink_Client.CustomWidgetsExtremLink_Client.CustomWidgets  5      '    !ExtremLink_Client.Pages.LoginPage   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7local%clr-namespace:ExtremLink_Client.Pages     5      GcustomWidgets-clr-namespace:ExtremLink_Client.CustomWidgets     5      .��5
      � ��5   
   �� 5        �� EndPoint$  0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5      �  � Color$ #00cec9��6    � Offset$ 0Q�6/   5      .+�5   
   �� $��Center=�6   $��Center=�60   $	��400��6M   $	��600��6Y   
������ .+�5      �� $!��/Images/ExtremLink_Icon.png��6   $	��150��69   $	��150��6E   $��0,20,0,0q�6R   �� 5      $��
ExtremLink��6   $��48(�6*   $
��Bold"�68   ���#031019$��Center=�6_   $��0,0,0,10q�6|   �� 5      $��Login��6   $��24(�6%   ���#031019$��Center=�6H   $��0,0,0,10q�6e   3   -ExtremLink_Client.CustomWidgets.CustomTextBox  5      -   5          Name$ usernameCustomTextBox��5   *      Placeholder$ Username��6I   $	��200��6`   $��40��6l   $��	0,20,0,30q�6x     5      -   5        $ passwordCustomTextBox��5   *   $ Password��6I   $	��200��6`   $��40��6l   $��0,0,0,10q�6x      IsHide$
 True��6�   �� 5      -   5         ��Name$ wrongLoginTextBlock��5      $y�Hidden=�65   $6��0Error: you entered a wrong username or password!��6I   
���  ��$��16(�6�   $��Center=�6�   $��0,0,0,10q�6�   2   ,ExtremLink_Client.CustomWidgets.CustomButton  5      2   ,ExtremLink_Client.CustomWidgets.CustomButton   CustomClick$ LoginButton_Click��6g   $	��100��6)   $��40��65   	   Placeholder$	 Login��6A   $��0,0,0,10q�6U   6g   �� 5      -   5        $��Create new user��5      $��0,3,0,8q�6/   $��14(�6A   ���#031019$��Center=�6d   
������ $��0q�6�   
 � Cursor$

 Hand��6�   6�   �� 5      -   5        $��Forgot password��5      $��0,3,0,8q�6/   $��14(�6A   ���#031019$��Center=�6d   
������ $��0q�6�   $

 Hand��6�   6�   5      5      5      