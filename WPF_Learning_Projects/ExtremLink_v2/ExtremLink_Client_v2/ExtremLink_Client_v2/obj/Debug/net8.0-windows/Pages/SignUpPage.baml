   M S B A M L   `   `   `  ����O  KExtremLink_Client_v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:ExtremLink_Client_v2.PagesExtremLink_Client_v2.Pages  5      W0clr-namespace:ExtremLink_Client_v2.CustomWidgets"ExtremLink_Client_v2.CustomWidgets  5      +    %ExtremLink_Client_v2.Pages.SignUpPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:ExtremLink_Client_v2.Pages     5      JcustomWidgets0clr-namespace:ExtremLink_Client_v2.CustomWidgets     5      .��5
      � ��5   
   �� 5        �� 
StartPoint$	  0,0&�6"    �� EndPoint$	 1,1&�63   .��5      �  � Color$ #031019��6    � Offset$ 0Q�6/   � 5      $ #034045��6   $	 0.5Q�6/   � 5      $ #026F71��6   $ 1Q�6/   5      .+�5   
   �� $��Center=�6   $��Center=�60   $	��400��6M   $	��600��6Y   
������ .+�5      �� $!��/Images/ExtremLink_Icon.png��6   $	��130��69   $	��130��6E   $��0,10,0,0q�6R   �� 5      $��
ExtremLink��6   $��48(�6*   $
��Bold"�68   ���#00CEC9$��Center=�6_   $��0,0,0,10q�6|   �� 5      $��Sign-up��6   $��24(�6'   ���#00CEC9$��Center=�6J   $��0,0,0,10q�6g   � 5       � ColumnDefinitions 5      �� 5      $��6* �6'   �� 5      $��1* �6'   �� 5      $��6* �6'    � RowDefinitions 5      �� 5      �� 5      �� 5       �� 5!      .+�5%      �� $��0��6   $��0��6-   .+�5&      6   0ExtremLink_Client_v2.CustomWidgets.CustomTextBox  -   5          Name$ fnCustomTextBox��5&   2      Placeholder$ 
first name��6K   $	��200��6d   $��40��6p   $��0,0,0,0q�6|      IsHide$
 True��6�   �� 5'      -   5        	 ��Name$	 wrongFirstNameTextBlock��5'       $y�hidden=�6A   $��Between 1 to 15 letters!��6U   
���  ��$��16(�6�   $
��Left=�6�   $��0,0,0,5q�6�   5(      �� 5+      $��0��6   $��1��6-   .+�5,        -   5        $ lnCustomTextBox��5,   2   $ 	last name��6K   $	��200��6c   $��40��6o   $��0,0,0,0q�6{   $
 True��6�   �� 5-      -   5        $	 wrongLastNameTextBlock��5-       $y�hidden=�6@   $��Between 1 to 15 letters!��6T   
���  ��$��16(�6�   $
��Left=�6�   $��0,0,0,5q�6�   5.      �� 51      $��0��6   $��2��6-   .+�52        -   5        $ cityCustomTextBox��52   2   $
 city��6M   $	��200��6`   $��40��6l   $��0,0,0,0q�6x   �� 53      -   5        $	 wrongCityTextBlock��53       $y�hidden=�6<   $��Between 5 to 15 letters!��6P   
���  ��$��16(�6�   $
��Left=�6�   $��0,0,0,5q�6�   54      �� 57      $��0��6   $��3��6-   .+�58        -   5        $ phoneCustomTextBox��58   2   $ phone (+972)��6N   $	��200��6i   $��40��6u   $��0,0,0,0q�6�   $
 True��6�   �� 59      -   5        $	 wrongPhoneTextBlock��59       $y�hidden=�6=   $��Should be 10 digits!��6Q   
���  ��$��16(�6~   $
��Left=�6�   $��0,0,0,5q�6�   5:      �� 5?      $��2��6   $��0��6-   .+�5@        -	   5        $ usernameCustomTextBox��5@   2   $ new user��6Q   $	��200��6h   $��40��6t   $��0,0,0,0q�6�   �� 5A      -
   5        $	 wrongUsernameTextBlock��5A       $y�hidden=�6@   $��Between 5-15 characters!��6T   
���  ��$��16(�6�   $
��Left=�6�   $��0,0,0,5q�6�   5B      �� 5E      $��2��6   $��1��6-   .+�5F        -   5        $ passwordCustomTextBox��5F   2   $ new password��6Q   $	��200��6l   $��40��6x   $��0,0,0,0q�6�   $
 True��6�   �� 5G      -   5        $	 wrongPasswordTextBlock��5G       $y�hidden=�6@   $��Between 8-20 characters!��6T   
���  ��$��16(�6�   $
��Left=�6�   $��0,0,0,5q�6�   5H      �� 5K      $��2��6   $��2��6-   .+�5L        -   5        $" passwordConfirmCustomTextBox��5L   2   $ confirm password��6X   $	��200��6w   $��40��6�   $��0,0,0,0q�6�   $
 True��6�   �� 5M      -   5        $#	 wrongPasswordConfirmTextBlock��5M       $y�hidden=�6G   $��Passwords don't match!��6[   
���  ��$��16(�6�   $
��Left=�6�   $��0,0,0,5q�6�   5N      �� 5Q      $��2��6   $��3��6-   .+�5R        -   5        $ emailCustomTextBox��5R   2   $ Email��6N   $	��200��6b   $��40��6n   $��0,0,0,0q�6z   $
 True��6�   �� 5S      -   5        $	 wrongEmailTextBlock��5S       $y�hidden=�6=   $��Should contain '@'!��6Q   
���  ��$��16(�6}   $
��Left=�6�   $��0,0,0,5q�6�   5T      5U      �� 5V      -   5        $	 errorSignUpTextBlock��5V      $y�Hidden=�66   $��This user already exist!��6J   
���  ��$��16(�6{   $��Center=�6�   $��0,0,0,0q�6�   �� 5W      -   5        
 ��Name$
 	signUpBtn��5W      $��Sign-Up��6(   $	��120��6:   $��45��6F   ���#034045
�������$��22(�5X      $��2q�6    � Cursor$
 Hand��63   ���#00CEC9$��Center=�5Y      $��Bottom=�6.   $��Consolas)�6I   $��	0,-15,0,0q�6_    J� Effect 5Z      3  -System.Windows.Media.Effects.DropShadowEffect  5[         Color$ #00CEC9��6'      
BlurRadius$ 20Q�67      ShadowDepth$ 0Q�6G   5]      5^      5_      5`      