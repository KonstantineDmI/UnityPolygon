h l i g h t   y o u r   c h o i c e ,   t h e n   p r e s s   E N T E R . )  
     < b r / >  
     < b r / >  
     < f o r m >  
     < p   p a d - l e f t = " 0 "   p a d - r i g h t = " 8 " >  
         < s e l e c t   n a m e = " o s b o o t - s e l e c t i o n "   m i n s i z e = " 6 "   t i p - t a r g e t = " a d v o p s - p r o m p t "   r i g h t - j u s t i f y = " t r u e "   s h o w - a r r o w s = " t r u e "   n o i n d e n t = " f a l s e " >  
             < x s l : f o r - e a c h   s e l e c t = " o s b o o t - e n t r y " >  
                 < x s l : e l e m e n t   n a m e = " o p t i o n " >  
                     < x s l : f o r - e a c h   s e l e c t = " @ d e f a u l t " >  
                         < x s l : a t t r i b u t e   n a m e = " s e l e c t e d " > t r u e < / x s l : a t t r i b u t e >  
                     < / x s l : f o r - e a c h >  
                 < x s l : a t t r i b u t e   n a m e = " v a l u e " > < x s l : v a l u e - o f   s e l e c t = " @ v a l u e " / > < / x s l : a t t r i b u t e >  
                 < x s l : a t t r i b u t e   n a m e = " t i p " > < x s l : v a l u e - o f   s e l e c t = " @ t i p " / > < / x s l : a t t r i b u t e >  
                 < x s l : a t t r i b u t e   n a m e = " c a r a t " > < x s l : v a l u e - o f   s e l e c t = " @ c a r a t " / > < / x s l : a t t r i b u t e >  
                     < x s l : v a l u e - o f   s e l e c t = " @ n a m e " / >  
                     < x s l : f o r - e a c h   s e l e c t = " @ e m s " >   [ E M S   E n a b l e d ] < / x s l : f o r - e a c h >  
                     < x s l : f o r - e a c h   s e l e c t = " @ d e b u g g e r " >   [ d e b u g g e r   e n a b l e d ] < / x s l : f o r - e a c h >  
                 < / x s l : e l e m e n t >  
             < / x s l : f o r - e a c h >  
         < / s e l e c t >  
         < b r / >  
         < / p >  
         < f o n t   f o r e g r o u n d - c o l o r = " R G B I " >  
         < t e x t a r e a   n a m e = " a d v o p s - p r o m p t "   w i d t h = " 7 6 "   h e i g h t = " 1 " / >  
         < / f o n t >  
         < b r / >  
         < x s l : f o r - e a c h   s e l e c t = " @ t i m e o u t " >  
             < t e x t a r e a   n a m e = " t i m e r s t r "   w i d t h = " 6 8 " > S e c o n d s   u n t i l   t h e   h i g h l i g h t e d   c h o i c e   w i l l   b e   s t a r t e d   a u t o m a t i c a l l y :   < / t e x t a r e a > < t e x t a r e a   n a m e = " t i m e r v a l "   w i d t h = " 8 " / >  
         < / x s l : f o r - e a c h >  
         < b r / >  
         < b r / >  
         < b r / >  
         < b r / >  
         < f o n t   f o r e g r o u n d - c o l o r = " R G B I " >  
         T o o l s :  
         < / f o n t >  
         < b r / >  
         < b r / >  
         < p   p a d - l e f t = " 0 "   p a d - r i g h t = " 8 " >  
           < s e l e c t   n a m e = " o s b o o t - t o o l - s e l e c t i o n "   m i n s i z e = " 3 "   t i p - t a r g e t = " a d v o p s - p r o m p t "   s h o w - a r r o w s = " t r u e "   r i g h t - j u s t i f y = " t r u e " >  
             < x s l : f o r - e a c h   s e l e c t = " o s b o o t - t o o l " >  
                 < x s l : e l e m e n t   n a m e = " o p t i o n " >  
                     < x s l : f o r - e a c h   s e l e c t = " @ d e f a u l t " >  
                         < x s l : a t t r i b u t e   n a m e = " s e l e c t e d " > t r u e < / x s l : a t t r i b u t e >  
                     < / x s l : f o r - e a c h >  
                 < x s l : a t t r i b u t e   n a m e = " v a l u e " > < x s l : v a l u e - o f   s e l e c t = " @ v a l u e " / > < / x s l : a t t r i b u t e >  
               < x s l : a t t r i b u t e   n a m e = " t i p " > < x s l : v a l u e - o f   s e l e c t = " @ t i p " / > < / x s l : a t t r i b u t e >  
                 < x s l : v a l u e - o f   s e l e c t = " @ n a m e " / >  
                 < / x s l : e l e m e n t >  
             < / x s l : f o r - e a c h >  
         < / s e l e c t >  
         < / p >  
     < / f o r m >  
     < / b o d y >  
 < / o s x m l : t e x t - m o d e - u i >  
 < / x s l : t e m p l a t e >  
  
 < x s l : t e m p l a t e   m a t c h = " o s b o o t - l i s t " >  
 < o s x m l : t e x t - m o d e - u i >  
     < h e a d >  
         < x s l : f o r - e a c h   s e l e c t = " @ n o w d s " >  
           < t i t l e   c o l o r = " X X X X R G B X " > < x s l : t e x t >                                                         W i n d o w s   B o o t   