   M S B A M L   `   `   `  ����C  ?Kursach2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Kursach2Kursach2  5   	       Kursach2.Cart   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Kursach2     5   	     � Title$
  Cart��5   	   $	��450��6   $	��800��6#    � 
ResizeMode$ NoResize=�6/   ��5	      �� 5
   
    �� ImageSource$ /background.jpg��6   .��5      �  � RowDefinitions 5   
   �� 5      $	��19* �6   �� 5      $
��416* �6   .+�5   
   �� $��2��6   �� 5   
   -   5         ��Name$ ProductScrollViewer��5      $��
50,81,50,0q�64   $��1��6H   $	��236��6U   $	��Top=�6b   ���	#CCFFFFFF.��5      �� -   5         ��Name$ ProductStackPanel��5      5      5      �� 5   
    �� CornerRadius$ 7��6   ���	#FF707070$��1q�6:   $��	10,10,0,0q�6N   
�������$��19��6t   $	��Top=�6�   $
��Left=�6�   $��78��6�   $��2��6�   ._�5      �� -   5        $��18��5      $��Center=�6!   $��2��6<   ���	#00DDDDDD���	#00707070
���   �$��Right=�6�   $��76��6�   .��6�   
Назад5      5      �� 5   
   $ 7��6   ���	#FF707070$��1q�6:   $��329,338,329,40q�6N   
�������$��1��6y   ._�5      �� -   5        $��35��5      $��Center=�6!   $��1��6N   ���	#00DDDDDD���	#00707070.��6�   Купить5      5      �� 5!   
   $��Корзина��6   $��24(�6#   $��266,10,266,334q�61   $��1��6I    �� TextAlignment$ Center=�6V   ���	#FFD64B4B6m   �� 5"   
   $��Общая цена:��6   $��18(�6'   $��50,322,620,52q�65   $��1��6L   $ Center=�6Y   
���   � J� RenderTransformOrigin$ 0.5,0.5&�6�   �� 5#   
   -   5        	 ��Name$	 
TotalPrice��5#   �   $��0��6   $��18(�6   $��185,322,0,31q�6+   $��1��6A   
���   �$ 0.5,0.5&�6a   $
��Left=�6�   $	��139��6�   6�   5%      5)      