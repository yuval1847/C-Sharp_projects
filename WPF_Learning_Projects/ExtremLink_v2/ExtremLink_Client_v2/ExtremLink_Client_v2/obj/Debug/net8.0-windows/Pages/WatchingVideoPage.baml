   M S B A M L   `   `   `  ����O  KExtremLink_Client_v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:ExtremLink_Client_v2.PagesExtremLink_Client_v2.Pages  5      W0clr-namespace:ExtremLink_Client_v2.CustomWidgets"ExtremLink_Client_v2.CustomWidgets  5      2    ,ExtremLink_Client_v2.Pages.WatchingVideoPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:ExtremLink_Client_v2.Pages     5      JcustomWidgets0clr-namespace:ExtremLink_Client_v2.CustomWidgets     5      .��5
      � ��5   
   �� 5        �� 
StartPoint$	  0,0&�6"    �� EndPoint$	 1,1&�63   .��5      �  � Color$ #031019��6    � Offset$ 0Q�6/   � 5      $ #034045��6   $	 0.5Q�6/   � 5      $ #026F71��6   $ 1Q�6/   5      .+�5   
   �� $��Center=�6   $	��562��60   .+�5      �� $��Watching Session 🎦��6   $��50(�63   $
��Bold"�6A   ���#00cec9$��Center=�5      $��0,5,0,20q�65   r� 5      -   5         r�Name$ VideoPlayer��5      $	��600��60   $��337.5��6<   $��0,0,0,50q�6K    r� LoadedBehavior$ Manual=�5       r� UnloadedBehavior$
 Stop=�6'    r� Stretch$ UniformToFill=�5      � 5#      $��Center=�6    � ColumnDefinitions 5$      �� 5%      $
��Auto �6'   �� 5&      $
��Auto �6'   �� 5'      $
��Auto �6'   .+�5+      �� -   5        $��▶ Play��5+      ���#034045
�������$	��130��6R   $��50��6^   $��22(�5,      $��2q�6#   	 � Cursor$
	 Hand��67   ���#00CEC9$��0��6[   $��Center=�5-      $��Center=�62   $��Consolas)�6M   $��0,0,30,0q�6c   
 J� Effect
 5/      3  -System.Windows.Media.Effects.DropShadowEffect  50         Color$ #00CEC9��6+      
BlurRadius$ 20Q�6;      ShadowDepth$ 0Q�6K   52      �� 55      -   5        $��	⏸ Pause��55      ���#034045
�������$	��130��6S   $��50��6_   $��22(�56      $��2q�6#   $
	 Hand��67   ���#00CEC9$��1��6[   $��Center=�57      $��Center=�62   $��Consolas)�6M   $��	30,0,30,0q�6c   
 59        5:      $ #00CEC9��6+   $ 20Q�6;   $ 0Q�6K   5<      �� 5?      -   5        $��⏹ Stop��5?      ���#034045
�������$	��130��6R   $��50��6^   $��22(�5@      $��2q�6#   $
	 Hand��67   ���#00CEC9$��2��6[   $��Center=�5A      $��Center=�62   $��Consolas)�6M   $��30,0,0,0q�6c   
 5C        5D      $ #00CEC9��6+   $ 20Q�6;   $ 0Q�6K   5F      5G      5H      �� 5K   
   $��Version 1.1��6   $��14(�6'   ���#00cec9$��Bottom=�6J   $��Right=�5L      $��	0,0,20,10q�60   5M      5N      