   M S B A M L   `   `   `  ����B  >WpfApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WpfApp2.ControlsWpfApp2.Controls  5      "    WpfApp2.Controls.MainControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:WpfApp2.Controls     5      -   5        .��5
      � $
��1024��6   $
��1280��6     � ColumnDefinitions  5   
   �� 5      $
��361* �6   �� 5      $
��919* �6   ��5   
   �� 5       �� ImageSource$ /ControlImages/背景.png��6    j� Stretch$ UniformToFill=�6=    � RowDefinitions 5   
   �� 5      $��245px �6   �� 5      $��250px �6   �� 5      $
��40px �6   �� 5      $��250px �6   �� 5      $
��auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��1��6/   $��Center=�6<   $��Center=�6Y   $��2��6t   $��290,0.4,290,0q�6�   .+�5      �� $	��320��6   $	��250��6!   #��[" �6.   $��0q�6i   ��6}   �� *SelectCommand+��6�   ��  �� ElementName$	 btn����5      �� 5      $��/ControlImages/查询.png��6   $
��Fill=�6;   6L   5      �� 5       $	��320��6   $	��250��6!   #��[" �6/   $��0q�6j   $��60,0,0,0q�6�   ��6~   �� *RenewCommand+��6�   �� $	 btn����5!      �� 5"      $��/ControlImages/续借.png��6   6=   5$      5%      �� 5&   
   $ 
Horizontal=�6   $��3��6/   $��Center=�6<   $��Center=�6Y   $��2��6t   $��290,0.2q�6�   .+�5'      �� $	��320��6   $	��250��6!   #��[" �6.   $��0q�6i   ��6~   �� *handleCommand+��6�   �� $	 btn����5(      �� 5)      $��/ControlImages/办卡.png��6   6=   5+      �� 5,      $	��320��6   $	��250��6!   #��[" �6.   $��0q�6i   $��60,0,0,0q�6}   ��6�   �� *PatchCardCommand+��6�   �� $	 btn����5-      �� 5.      $(��"/ControlImages/挂失／补办.png��6   6@   50      51      �� 52   
   $��125,108.8,0,-108q�6   $��1��6+   $��4��6;   .+�53      �� -   5        $
��date��53      $��35(�6    
�������$��0,0,0,0q�6A   .��6R   
2019-3-56\   �� 54      -   5        $
��time��54      $��35(�6    
�������$��	205,0,0,0q�6A   .��6T   17:156[   55      57      58      