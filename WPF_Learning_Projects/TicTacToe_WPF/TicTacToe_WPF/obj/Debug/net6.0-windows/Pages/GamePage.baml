   M S B A M L   `   `   `  ����H  DTicTacToe_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:TicTacToe_WPF.PagesTicTacToe_WPF.Pages  5      "    TicTacToe_WPF.Pages.GamePage   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:TicTacToe_WPF.Pages     5      .��5	      � 
�������  � RowDefinitions  5
   
   �� 5      $
��0.2* �6   �� 5      $
��0.6* �6   �� 5      $
��0.2* �6    � ColumnDefinitions 5   
   �� 5      $��0.25* �6   �� 5      $
��0.5* �6   �� 5      $��0.25* �6   .+�5   
   �� -   5         ��Name$ turnTextBox��5      $��0��6)   $��1��66   $��Turn of player: x��6F   $��50(�6_   $��Center=�6m   $��Center=�5      �� 5   
   -   5         ��Name$ winningLine��5      $��1��6%   $��1��62    K� ZIndex$ 1��6B   $��Center=�5      $��Center=�6+   �� 5   
   -   5         ��Name$ 
TopLeftBtn��5      $	��100��6%   $	��100��61   $
��Left=�6>   $	��Top=�6Y   
������ $��1��5   "   $��1��62   $��0q�6?   .��5      �� -   5        $ TopLeftImage��5      $��/Images/empty_cell.png��6*   $	��100��6J   $	��100��6V   $��Center=�5       $��Center=�6'   5!      �� 5#   
   -   5        $ TopCenterBtn��5#      $	��100��6'   $	��100��63   $��Center=�6@   $	��Top=�6]   
������ $��1��5$   "   $��1��62   $��0q�6?   .��5&      �� -   5        $ TopCenterImage��5&      $��/Images/empty_cell.png��6,   $	��100��6L   $	��100��6X   $��Center=�5'      $��Center=�6'   5(      �� 5*   
   -   5        $ TopRightBtn��5*      $	��100��6&   $	��100��62   $��Right=�6?   $	��Top=�6[   
������ $��1��5+   "   $��1��62   $��0q�6?   .��5-      �� -   5        $ TopRightImage��5-      $��/Images/empty_cell.png��6+   $	��100��6K   $	��100��6W   $��Center=�5.      $��Center=�6'   5/      �� 52   
   -	   5        $ CenterLeftBtn��52      $	��100��6(   $	��100��64   $
��Left=�6A   $��Center=�6\   
������ $��1��53   "   $��1��62   $��0q�6?   .��55      �� -
   5        $ CenterLeftImage��55      $��/Images/empty_cell.png��6-   $	��100��6M   $	��100��6Y   $��Center=�56      $��Center=�6'   57      �� 59   
   -   5        $ CenterCenterBtn��59      $	��100��6*   $	��100��66   $��Center=�6C   $��Center=�6`   
������ $��1��5:   "   $��1��62   $��0q�6?   .��5<      �� -   5        $ CenterCenterImage��5<      $��/Images/empty_cell.png��6/   $	��100��6O   $	��100��6[   $��Center=�5=      $��Center=�6'   5>      �� 5@   
   -   5        $ CenterRightBtn��5@      $	��100��6)   $	��100��65   $��Right=�6B   $��Center=�6^   
������ $��1��5A   "   $��1��62   $��0q�6?   .��5C      �� -   5        $ CenterRightImage��5C      $��/Images/empty_cell.png��6.   $	��100��6N   $	��100��6Z   $��Center=�5D      $��Center=�6'   5E      �� 5H   
   -   5        $ BottomLeftBtn��5H      $	��100��6(   $	��100��64   $
��Left=�6A   $��Bottom=�6\   
������ $��1��5I   "   $��1��62   $��0q�6?   .��5K      �� -   5        $ BottomLeftImage��5K      $��/Images/empty_cell.png��6-   $	��100��6M   $	��100��6Y   $��Center=�5L      $��Center=�6'   5M      �� 5O   
   -   5        $ BottomCenterBtn��5O      $	��100��6*   $	��100��66   $��Center=�6C   $��Bottom=�6`   
������ $��1��5P   "   $��1��62   $��0q�6?   .��5R      �� -   5        $ BottomCenterImage��5R      $��/Images/empty_cell.png��6/   $	��100��6O   $	��100��6[   $��Center=�5S      $��Center=�6'   5T      �� 5V   
   -   5        $ BottomRightBtn��5V      $	��100��6)   $	��100��65   $��Right=�6B   $��Bottom=�6^   
������ $��1��5W   "   $��1��62   $��0q�6?   .��5Y      �� -   5        $ BottomRightImage��5Y      $��/Images/empty_cell.png��6.   $	��100��6N   $	��100��6Z   $��Center=�5Z      $��Center=�6'   5[      5]      5^      