  b o o t   e n t r y   i d   t o   c h a n g e 
   /                                                       t h e   O S   o p t i o n s   t o . 
 
 8         / ?                                           D i s p l a y s   t h i s   h e l p   m e s s a g e . 
 
 
 E x a m p l e s : 
 %         B O O T C F G   / D B G 1 3 9 4   O N   / C H   3 0   / I D   3 
 7         B O O T C F G   / D B G 1 3 9 4   O N   / C H   2 0   / S   s y s t e m   / U   u s e r   / I D   2 
           B O O T C F G   / D B G 1 3 9 4   O F F   / I D   2   
     PA              D         B O O T C F G   / D B G 1 3 9 4   O F F   / S   s y s t e m   / U   d o m a i n \ u s e r   / P   p a s s w o r d   / I D   2 
 g E R R O R :   C a n n o t   r e m o v e   t h e   / d b g 1 3 9 4   s w i t c h   w i t h   t h i s   o p t i o n . 
 U s e   / D B G 1 3 9 4   s w i t c h   f o r   r e m o v i n g   1 3 9 4   p o r t . 
 6 E R R O R :   T h e   t a r g e t   s y s t e m   m u s t   b e   r u n n i n g   a   3 2   b i t   O S . 
  E R R O R :   A c c e s s   D e n i e d . 
   8 E R R O R :   I n v a l i d   s y n t a x .   M a n d a t o r y   s w i t c h   / I D   i s   m i s s i n g . 
 " T y p e   " B O O T C F G   / E M S   / ? "   f o r   u s a g e . 
 d E R R O R :   C a n n o t   e d i t   t h e   / D B G 1 3 9 4   s w i t c h   w i t h   t h i s   o p t i o n . 
 U s e   / D B G 1 3 9 4   s w i t c h   f o r   e d i t i n g   1 3 9 4   p o r t . 
 & T y p e   " B O O T C F G   / D B G 1 3 9 4   / ? "   f o r   u s a g e . 
 PAC E R R O R :   B a u d   r a t e   i s   n o t   p r e s e n t .   A d d   a   b a u d   r a t e   a n d   t h e n   p r o c e e d . 
 4 E R R O R :   I n v a l i d   s y n t a x .   D e f a u l t   a r g u m e n t   i s   m i s s i n g . 
 G E R R O R :   R e m o t e   b o o t   c o n f i g u r a t i o n   i s   n o t   s u p p o r t e d   f o r   6 4   b i t   m a c h i n e s . 
 	 W A R N I N G :   < E R R O R :   B a u d r a t e   c a n n o t   b e   a d d e d   w i t h o u t   a d d i n g   a   d e b u g p o r t . 
 , E R R O R :   T h e   / d b g 1 3 9 4   s w i t c h   a l r e a d y   p r e s e n t . 
                     PA4 E R R O R :   U n e x p e c t e d   e r r o r   f r o m   N t Q u e r y B o o t E n t r y O r d e r . 
 5 E R R O R :   U n e x p e c t e d   e r r o r   f r o m   N t E n u m e r a t e B o o t E n t r i e s . 
 1 E R R O R :   U n e x p e c t e d   e r r o r   f r o m   N t Q u e r y B o o t O p t i o n s . 
 0 E R R O R :   U n e x p e c t e d   e r r o r   f r o m   N t M o d i f y B o o t E n t r y . 
 " E R R O R :   U n e x p e c t e d   e r r o r   o c c u r r e d . 
 2 E R R O R :   U n e x p e c t e d   e r r o r   f r o m   N t S e t B o o t E n t r y O r d e r . 
   E R R O R :   I n s u f f i c i e n t   p r i v i l e g e s . 
 0 E R R O R :   T h i s   v e r s i o n   d o e s   n o t   s u p p o r t   E F I   N V R A M . 
 ! E R R O R :   D e b u g p o r t   d o e s   n o t   e x i s t . 
 = E R R O R :   U n e x p e c t e d   e r r o r   o c c u r r e d   w h e n   p a r s i n g   B O O T . I N I   f i l e . 
                             @           
 B O O T C F G   / M i r r o r   / A D D   g u i d   [ / D   d e s c r i p t i o n ]   [ / I D   b o o t i d ]   
 
    D e s c r i p t i o n : 
 G         A l l o w s   a n   a d m i n i s t r a t o r   t o   a d d   a   b o o t   e n t r y   f o r   a   m i r r o r e d   d r i v e . 
 
  P a r a m e t e r   L i s t : 
 ?         / A D D         g u i d                 A d d s   a   n e w   b o o t   e n t r y   f o r   t h e   m i r r o r e d 
 <                                                 p a r t i t i o n   w i t h   t h e   s p e c i f i e d   G U I D . 
 
 H         / D             d e s c r i p t i o n   T h e   d e s c r i p t i o n   o f   t h e   b o o t   e n t r y   b e i n g   a d d e d . 
 
 PAH         / I D           b o o t i d             T h e   b o o t   e n t r y   w h o s e   l o a d e r   p a t h   i s   t o   b e   u s e d . 
 <                                                 D e f a u l t s   t o   c u r r e n t   b o o t   e n t r y   i d . 
 
 5         / ?                                     D i s p l a y s   t h i s   h e l p   m e s s a g e . 
 
 
 E x a m p l e s : 
 >       B O O T C F G   / M I R R O R   / A D D   0 9 e a 8 b a 0 - a 1 2 2 - 0 1 c 0 - f 1 b 3 - 1 2 7 1 4 f 7 5 8 8 2 1   
 )                       / D   " M i r r o r e d   O S   E n t r y "   / I D   3   
             F       B O O T C F G   / M I R R O R   / A D D   { 0 9 e a 8 b a 0 - a 1 2 2 - 0 1 c 0 - f 1 b 3 - 1 2 7 1 4 f 7 5 8 8 2 1 }   / I D   2   
 * E R R O R :   T h e   O S   o p t i o n   m u s t   s t a r t   w i t h   " / " . 
  E R R O R :   F i l e   n o t   f o u n d . 
   PA B o o t   O p t i o n s 
  - - - - - - - - - - - - 
  T i m e o u t :                           % d 
  C u r r e n t B o o t E n t r y I D :     % d 
  H e a d l e s s R e d i r e c t i o n :   N o t   s e t 
 
  H e a d l e s s R e d i r e c t i o n :   % s 
 
  B o o t