   M S B A M L   `   `   `  ���� E  AJSON Nexus, Version=4.2.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:JSON_Nexus
JSON_Nexus      JSON_Nexus.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:JSON_Nexus       � Title$  
JSON Nexus��$	��800��$
��1020�� � 
ResizeMode$ CanMinimize=�.��� .+��� .@��� $��	Read JSON��.��� ���	#FFE5E5E5.+��� -   $��locationOfJSON��$	��Top=�$
��Left=�$��25��$	��800��$��Roboto)�$��15(� �� TextAlignment$
 Left=�$��Center=�
�������.��-+C:/path/to/json or https://example.com/json�� -   $	��Top=�$��Right=�$��25��$��90��.��Open�� -   $	��Top=�$��Right=�$��25��$��	0,0,100,0q�$��90��.��Read�� -   $��enableTB��$	��Top=�$
��Left=�$��0,35,0,0q�{�.  .��Unlock For Editing�� -   $��saveBTN��$	��Top=�$
��Left=�{�.  $��25��$��50��$��
150,30,0,0q�$��Center=�$��Center=�.��Save�� -   $��findBTN��$	��Top=�$
��Left=�$��25��$��50��$��
210,30,0,0q�{�.  $��Center=�$��Center=�.��Find�� -   $��
findObject��$	��Top=�$
��Left=�$	��100��$��25��$��
270,30,0,0q�{�.  $
 Left=�$��Center=�$��Center=��� -   $��readPathBTN��$	��Top=�$
��Left=�$��25��$��70��$��
380,30,0,0q�{�.  $��Center=�$��Center=�.��	Read Path�� -	   $��
readObject��$	��Top=�$
��Left=�$	��200��$��25��$��
460,30,0,0q�{�.  $
 Left=�$��Center=�$��Center=��� -
   $��
ReplaceBTN��$	��Top=�$
��Left=�$��25��$��70��$��
670,30,0,0q�{�.  $��Center=�$��Center=�.��	Replace�� -   $��insteadOfObject��$	��Top=�$
��Left=�$��70��$��25��$��
750,30,0,0q�{�.  $
 Left=�$��Center=�$��Center=��� -   $��withtb��$��Right=�$	��Top=�$��
0,30,140,0q�
�������.��With�� -   $��
withObject��$	��Top=�$
��Left=�$��70��$��25��$��
860,30,0,0q�{�.  $
 Left=�$��Center=�$��Center=��� $��0,60,0,0q�.���� -   $��JsonData��$
��Auto��$��15(�$
��Left=�|�.  �� AcceptsReturn .  �� 
IsReadOnly . �� $��Create JSON��.��� ���	#FFE5E5E5.+��� $��	10,14,0,0q�$
��Left=�$	��Top=�$��Name of property���� -   $��nameOfProperty��$��
115,10,0,0q�$
��Left=�$	��Top=�$��25��$	��150��$
��Left=�$��Center=��� $��
270,14,0,0q�$
��Left=�$	��Top=�$��Value of property���� -   $��valueOfProperty��$��
370,10,0,0q�$
��Left=�$	��Top=�$��25��$	��150��$
��Left=�$��Center=��� $��
100,14,0,0q�$��Center=�$	��Top=�$��DateType���� -   $��dataTypeCBox��$��
240,10,0,0q�$��Center=�$	��Top=�$��25��$��80��$��Center=�$��Center=�$��11(�.@��� $��string���� $	��int���� $��boolean���� $��double���� $��decimal���� $��float���� -   $��
AddItemBTN��$��
0,10,230,0q�$��25��$	��100��$��Right=�$	��Top=�$��Center=�$��Center=�$��Add Value(s)���� -   $��AddJsonObjectBTN��$��
0,10,120,0q�$��25��$	��100��$��Right=�$	��Top=�$��Center=�$��Center=�$��Create Object���� -   $��
ConvertBTN��$��	0,10,10,0q�$��25��$	��100��$��Right=�$	��Top=�$��Center=�$��Center=�$��Convert���� $��Commands��$	��Top=�$
��Left=�$��
245,50,0,0q��� $
��JSON��$	��Top=�$��Right=�$��
0,50,245,0q��� -    ��Name$ itemstb��$��0,75,0,0q�$	��490��$
��Left=�|�.  .  �� TextWrapping$
 Wrap=��� -   $ jsontb��$��0,75,0,0q�$	��490��$��Right=�|�.  . $
 Wrap=��� $��Web Requests��.��� ���	#FFE5E5E5.+��� -   $��urlTXT��$	��400��$��25��$
��Left=�$��Center=�$
��Left=�$	��Top=�$��	10,15,0,0q��� $	��URL��$��12(�$
��Left=�$	��Top=�$��
205,40,0,0q��� -   $��authTXT��$	��400��$��25��$
��Left=�$��Center=�$��Center=�$	��Top=�$��
230,15,0,0q��� $��Authorization Header��$��12(�$��Center=�$	��Top=�$��
230,40,0,0q��� -   $��RequestMethod��$��80��$��25��$
��Left=�$��Center=�$��Right=�$	��Top=�$��
0,15,100,0q�.@��� $	��GET���� $
��POST���� $	��PUT���� $��PATCH���� $��DELETE���� $��OPTIONS���� -   $��SendBTN��$��80��$��25��$��Right=�$	��Top=�$��	0,15,10,0q�.��Send Request�� -   $��
headersTXT��$��0,100,550,380q�$
 Wrap=� . �� -   $��AddDefaultHeaders��$��1,70,0,0q�$	��120��$��25��$	��Top=�$
��Left=�$��Center=�$��Center=�.��Add Default Headers�� $��Headers��$��12(�$
��Left=�$	��Top=�$��
205,80,0,0q��� -   $��bodyTXT��$��0,420,550,0q�$
 Wrap=� . �� $
��Body��$��12(�$
��Left=�$	��Top=�$��205,400,0,0q��� -   $��LoadPayload��$	��100��$��25��$
��Left=�$��Center=�$��1,70,0,0q�.��Load Payload�� -   $��
AddCookies��$	��100��$��25��$��Center=�$	��Top=�$��
0,70,202,0q�.��Add Cookies�� -    $��ClearCookies��$	��100��$��25��$��Center=�$	��Top=�$��
0,70,-20,0q�.��Clear Cookies�� -!   $��Clear��$	��100��$��25��$��Right=�$	��Top=�$��0,70,1,0q�.��Clear�� -"   $��BodyType��$��80��$��25��$
��Left=�$��Center=�$��Center=�$��Center=�$��
0,70,182,0q�.@��� $
��JSON���� $��Form URL Encoded���� $��Text / Plain���� -#   $��responseTXT��$��460,100,0,0q� . �� $��Response��$��12(�$��Right=�$	��Top=�$��
0,80,250,0q�