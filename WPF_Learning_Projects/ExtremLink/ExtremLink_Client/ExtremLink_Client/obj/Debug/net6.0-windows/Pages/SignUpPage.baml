   M S B A M L   `   `   `  ����L  HExtremLink_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:ExtremLink_Client.PagesExtremLink_Client.Pages  5      Q-clr-namespace:ExtremLink_Client.CustomWidgetsExtremLink_Client.CustomWidgets  5      (    "ExtremLink_Client.Pages.SignUpPage   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7local%clr-namespace:ExtremLink_Client.Pages     5      GcustomWidgets-clr-namespace:ExtremLink_Client.CustomWidgets     5      .��5
      � ��5   
   �� 5        �� EndPoint$  0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5      �  � Color$ #00cec9��6    � Offset$ 0Q�6/   5      .+�5   
   �� $��Center=�6   $��Center=�60   $	��400��6M   $	��600��6Y   
������ .+�5      �� $!��/Images/ExtremLink_Icon.png��6   $	��130��69   $	��130��6E   $��0,10,0,0q�6R   �� 5      $��
ExtremLink��6   $��48(�6*   $
��Bold"�68   ���#031019$��Center=�6_   $��0,0,0,10q�6|   �� 5      $��Sign-up��6   $��24(�6'   ���#031019$��Center=�6J   $��0,0,0,10q�6g   � 5       � ColumnDefinitions 5      �� 5      $��6* �6'   �� 5      $��1* �6'   �� 5      $��6* �6'    � RowDefinitions 5      �� 5      �� 5      �� 5      �� 5      .+�5#      �� $��0��6   $��0��6-   .+�5$      3   -ExtremLink_Client.CustomWidgets.CustomTextBox  -   5          Name$ fnCustomTextBox��5$   2      Placeholder$ 
first name��6K   $	��200��6d   $��40��6p   $��0,0,0,0q�6|      IsHide$
 True��6�   �� 5%      -   5        	 ��Name$	 wrongFirstNameTextBlock��5%       $y�hidden=�6A   $��Between 1 to 15 letters!��6U   
���  ��$��16(�6�   $
��Left=�6�   $��0,0,0,5q�6�   5&      �� 5)      $��0��6   $��1��6-   .+�5*        -   5        $ lnCustomTextBox��5*   2   $ 	last name��6K   $	��200��6c   $��40��6o   $��0,0,0,0q�6{   $
 True��6�   �� 5+      -   5        $	 wrongLastNameTextBlock��5+       $y�hidden=�6@   $��Between 1 to 15 letters!��6T   
���  ��$��16(�6�   $
��Left=�6�   $��0,0,0,5q�6�   5,      �� 5/      $��0��6   $��2��6-   .+�50        -   5        $ cityCustomTextBox��50   2   $
 city��6M   $	��200��6`   $��40��6l   $��0,0,0,0q�6x   �� 51      -   5        $	 wrongCityTextBlock��51       $y�hidden=�6<   $��Between 5 to 15 letters!��6P   
���  ��$��16(�6�   $
��Left=�6�   $��0,0,0,5q�6�   52      �� 55      $��0��6   $��3��6-   .+�56        -   5        $ phoneCustomTextBox��56   2   $ phone(+972)��6N   $	��200��6h   $��40��6t   $��0,0,0,0q�6�   $
 True��6�   �� 57      -   5        $	 wrongPhoneTextBlock��57       $y�hidden=�6=   $��Should be 10 digits!��6Q   
���  ��$��16(�6~   $
��Left=�6�   $��0,0,0,5q�6�   58      �� 5=      $��2��6   $��0��6-   .+�5>        -	   5        $ usernameCustomTextBox��5>   2   $ new user��6Q   $	��200��6h   $��40��6t   $��0,0,0,0q�6�   �� 5?      -
   5        $	 wrongUsernameTextBlock��5?       $y�hidden=�6@   $��Between 5-15 characters!��6T   
���  ��$��16(�6�   $
��Left=�6�   $��0,0,0,5q�6�   5@      �� 5C      $��2��6   $��1��6-   .+�5D        -   5        $ passwordCustomTextBox��5D   2   $ new password��6Q   $	��200��6l   $��40��6x   $��0,0,0,0q�6�   $
 True��6�   �� 5E      -   5        $	 wrongPasswordTextBlock��5E       $y�hidden=�6@   $��Between 8-20 characters!��6T   
���  ��$��16(�6�   $
��Left=�6�   $��0,0,0,5q�6�   5F      �� 5I      $��2��6   $��2��6-   .+�5J        -   5        $" passwordConfirmCustomTextBox��5J   2   $ confirm password��6X   $	��200��6w   $��40��6�   $��0,0,0,0q�6�   $
 True��6�   �� 5K      -   5        $#	 wrongPasswordConfirmTextBlock��5K       $y�hidden=�6G   $��Passwords don't match!��6[   
���  ��$��16(�6�   $
��Left=�6�   $��0,0,0,5q�6�   5L      �� 5O      $��2��6   $��3��6-   .+�5P        -   5        $ emailCustomTextBox��5P   2   $ Email��6N   $	��200��6b   $��40��6n   $��0,0,0,0q�6z   $
 True��6�   �� 5Q      -   5        $	 wrongEmailTextBlock��5Q       $y�hidden=�6=   $��Should contain '@'!��6Q   
���  ��$��16(�6}   $
��Left=�6�   $��0,0,0,5q�6�   5R      5S      �� 5T      -   5        $	 errorSignUpTextBlock��5T      $y�Hidden=�66   $��This user already exist!��6J   
���  ��$��16(�6{   $��Center=�6�   $��0,0,0,10q�6�   2   ,ExtremLink_Client.CustomWidgets.CustomButton  5U      -   5        
  Name$
 	signUpBtn��5U   )   2   ,ExtremLink_Client.CustomWidgets.CustomButton   CustomClick$ SignUpButton_Click��6|   $	��100��6<   $��40��6H      Placeholder$ Sign-Up��6T   $��0,0,0,10q�6j   6|   5V      5W      5X      