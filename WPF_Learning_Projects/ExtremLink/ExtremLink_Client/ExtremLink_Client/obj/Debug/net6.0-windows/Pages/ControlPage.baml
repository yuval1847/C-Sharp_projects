   M S B A M L   `   `   `  ����L  HExtremLink_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:ExtremLink_Client.PagesExtremLink_Client.Pages  5      )    #ExtremLink_Client.Pages.ControlPage   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7local%clr-namespace:ExtremLink_Client.Pages     5      .��5	      � ��5
   
   �� 5        �� 
StartPoint$	  0,0&�6"    �� EndPoint$	 1,1&�63   .��5      �  � Color$ #031019��6    � Offset$ 0Q�6/   � 5      $ #034045��6   $	 0.5Q�6/   � 5      $ #026F71��6   $ 1Q�6/   5      .+�5   
   �� .+�5      �� -   5         ��Name$ frameImg��5      $��/Images/default_image.png��6&   $	��800��6I   $	��450��6U   $	��Top=�6b   $��Center=�6z   5      � 5       � ColumnDefinitions 5      �� 5      �� 5      �� 5      �� 5       � RowDefinitions 5      �� 5      $	��100 �6$   �� 5       .+�5$      �� $��5,-445,120,496q�6   ���	#90000000$��2��6E   �� 5%      $��/Images/pc_icon.png��6   $��1��65   $��20��6I   $��20��6T   $��7,-445,168,520q�6`    J� RenderTransformOrigin$ 0.496,0.508&�6x   �� 5&      -   5         ��Name$ clientIpTextBlock��5&      $��Consolas)�67   $��Victim's IP:��6M   $	��Top=�6a   ���#00cec9$��15(�6�   $��2��6�   $��35,-441,125,0q�6�   �� 5'      $��/Images/video_icon_1.png��6   $��1��6:   $��20��6N   $��20��6Y   $��7,-425,168,500q�6e   $ 
0.496,0.58&�6}   �� 5(      -   5        $ recordingStatusTextBlock��5(      $��Consolas)�6>   $��Record: off��6T   $	��Top=�6g   ���#00cec9$��15(�6�   $��2��6�   $��35,-418,192,0q�6�   �� 5,      -   5        $��▶ Play��5,      ���#034045
�������$	��130��6R   $��50��6^   $��22(�5-      $��2q�6#   	 � Cursor$
	 Hand��67   ���#00CEC9$��0��6[   $��Center=�5.      $��Center=�62   $��Consolas)�6M   $��0,0,0,0q�6c   
 J� Effect
 50      3  -System.Windows.Media.Effects.DropShadowEffect  51         Color$ #00CEC9��6+      
BlurRadius$ 20Q�6;      ShadowDepth$ 0Q�6K   53      �� 56      -   5        $��	⏸ Pause��56      ���#034045
�������$	��130��6S   $��50��6_   $��22(�57      $��2q�6#   $
	 Hand��67   ���#00CEC9$��1��6[   $��Center=�58      $��Center=�62   $��Consolas)�6M   $��0,0,0,0q�6c   
 5:        5;      $ #00CEC9��6+   $ 20Q�6;   $ 0Q�6K   5=      �� 5@      -   5        $��⏹ Stop��5@      ���#034045
�������$	��130��6R   $��50��6^   $��22(�5A      $��2q�6'   $
	 Hand��6;   ���#00CEC9$��2��6_   $��Center=�5B      $��Center=�66   $��Consolas)�6Q   $��0,0,0,0q�6g   
 5D        5E      $ #00CEC9��6+   $ 20Q�6;   $ 0Q�6K   5G      �� 5J      -   5        $��
⚫ Record��5J      ���#034045
�������$	��130��6T   $��50��6`   $��22(�5K      $��2q�6'   $
	 Hand��6;   ���#00CEC9$��3��6_   $��Center=�5L      $��Center=�66   $��Consolas)�6Q   $��0,0,0,0q�6g   
 5N        5O      $ #00CEC9��6+   $ 20Q�6;   $ 0Q�6K   5Q      5S      5T      �� 5U   
   $��Version 1.1��6   $��14(�6'   $��Consolas)�65   ���#00cec9$��Bottom=�6`   $��Right=�6{   $��	0,0,20,20q�6�   5W      5X      