   M S B A M L   `   `   `  ����C  ?Kursach2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Kursach2Kursach2  5   	       Kursach2.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Kursach2     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)    � 
ResizeMode$ NoResize=�65   ��5	      �� 5
   
    �� ImageSource$ /background.jpg��6    � 	Resources 5         brushWatermarkBackground �� Color  brushWatermarkForeground  brushWatermarkBorder   BooleanToVisibilityConverter  EntryFieldStyle �� 
TargetType �� Property �� Value��%!  &	        &	      &	 G     &	 g     &	 z     �� 5   
   $ White��6;   �� 5   
   $ LightSteelBlue��6;   �� 5   
   $ Indigo��67   �� 5   
   6'   �� 5   
   " �6(   .�5      ��  � ��$ Stretch=�64   �� 5       � ��$ Center=�62   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $	��54* �6   �� 5      $
��163* �6   	 � ColumnDefinitions	 5    
   �� 5!      $
��199* �6   �� 5"      $
��201* �6   .+�5$   
   �� $��2��6   $��2��6)   �� 5%   
   -   5        
 ��Name$
 ProductScrollViewer��5%      $��50,100,50,50q�64   $��2��6J   $��2��6^   ���	#CCFFFFFF.��5&      �� -   5         ��Name$ ProductStackPanel��5&      5'      5(      �� 5*   
   -   5         ��Name$ FilterTextBox��5*      $	��200��6R   $��30��6^   $��300,47,300,31q�6j   #��[ 6�   $��2��6�   �� 5+   
   #��[ 6    �� TextAlignment$ Center=�5-      $��371,49,371,34q�6/   $��2��6F    � ForceCursor . y�5,      �� *Text.IsEmpty+ �� 	Converter# [  �� ElementName$ FilterTextBox���� 5-   o    � Language$ ru-ru�6s   $�
Поиск��6�   6�   �� 5/   
   -   5         ��Name$ CategoryComboBox��5/   )   $��20,47,152,31q�6   .@�6x   Нет50      �� 51   
   #��[ 6   $ Center=�53      $��104,50,236,33q�6(    . y�52      �� *Text.IsEmpty+# [ $ CategoryComboBox���� 53   T   $ ru-ru�6X   $�Категория��6i   6}   �� 55   
   -   5        $��296,16,37,30q�55      $��1��6'   .��56      �� $��user.png��6   57      �� 59   
   -   5        $ 	MenuPanel��59   \   $��1��6   $��0,78,0,0q�6%   $��2��67   $y�Hidden=�6H   
�������$	��148��6�   $	��Top=�6�   $��Right=�6�   $	��144��6�   .+�5:      �� -   5        $��38��5:      .��62   Корзина5;      �� 5<      -   5        $��38��5<      .��65   профиль5=      �� 5>      -	   5        $��38��5>      .��6?   Истроия покупок5?      �� 5@      -
   5        $��38��5@      .��69    Выйти из профиля5A      5B      5D      5E      