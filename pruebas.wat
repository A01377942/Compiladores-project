;; WebAssembly text format code generated by the QuetzalDragon compiler.

(module
  (import "quetzal" "printi" (func $printi (param i32) (result i32)))
  (import "quetzal" "printc" (func $printc (param i32) (result i32)))
  (import "quetzal" "prints" (func $prints (param i32) (result i32)))
  (import "quetzal" "println" (func $println (result i32)))
  (import "quetzal" "readi" (func $readi (result i32)))
  (import "quetzal" "reads" (func $reads (result i32)))
  (import "quetzal" "new" (func $new (param i32) (result i32)))
  (import "quetzal" "size" (func $size (param i32) (result i32)))
  (import "quetzal" "add" (func $add (param i32 i32) (result i32)))
  (import "quetzal" "get" (func $get (param i32 i32) (result i32)))
  (import "quetzal" "set" (func $set (param i32 i32 i32) (result i32)))

  (func $sqr
  (param $x i32)
  (result i32)
   (local $_temp i32)
    local.get $x
    local.get $x
     i32.mul
   return
  )
  (func
    (export "main")
    (result i32)
   (local $_temp i32)
    (local $array i32)
    (local $sum i32)
    (local $i i32)
    (local $j i32)
    (local $x i32)
  ;; Arreglo
    i32.const 0
    call $new
    local.set $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
   local.get $_temp
    i32.const 1
    i32.const 1
     i32.add
   call $add
   drop
    i32.const 2
    i32.const 3
     i32.mul
    i32.const 2
     i32.mul
   call $add
   drop
    i32.const 2
    i32.const 3
   call $sqr
     i32.mul
    i32.const 2
     i32.add
   call $add
   drop
    i32.const 20
    i32.const 2
    i32.const 2
     i32.add
    i32.const 4
     i32.eq
     i32.sub
   call $add
   drop
    i32.const 5
   call $add
   drop
    i32.const 4
    i32.const 2
     i32.mul
   call $add
   drop
    i32.const 2
    i32.const 8
     i32.mul
   call $add
   drop
    i32.const 2
    i32.const 2
     i32.add
    i32.const 5
     i32.eq
   call $add
   drop
    i32.const 5
   call $sqr
    i32.const 2
     i32.sub
   call $add
   drop
    i32.const 5
    i32.const 2
     i32.mul
    i32.const 1
     i32.add
   call $add
   drop
    i32.const 1
    i32.const 4
   call $sqr
     i32.add
   call $add
   drop
    i32.const 3
     i32.sub
    i32.const 2
   call $sqr
   call $sqr
     i32.add
   call $add
   drop
    i32.const 10
    i32.const 8
     i32.add
   call $add
   drop
    i32.const 30
    i32.const 6
     i32.sub
    i32.const 2
     i32.sub
   call $add
   drop
    i32.const 2
    i32.const 3
    i32.const 2
     i32.mul
    i32.const 1
     i32.sub
     i32.mul
   call $add
   drop
    i32.const 3
   call $sqr
    i32.const 5
    i32.const 2
     i32.mul
    i32.const 1
     i32.add
     i32.mul
   call $add
   drop
    i32.const 8
    i32.const 7
     i32.mul
   call $add
   drop
    i32.const 4
   call $sqr
   call $add
   drop
    i32.const 2
    i32.const 3
     i32.add
    i32.const 2
    i32.const 3
     i32.mul
     i32.lt_s
   call $add
   drop
    i32.const 1
     i32.sub
    i32.const 2
    i32.const 2
     i32.mul
     i32.add
   call $add
   drop
    i32.const 2
   call $sqr
   call $add
   drop
    i32.const 3
    i32.const 4
     i32.add
    i32.const 2
     i32.mul
   call $add
   drop
    i32.const 10
     i32.sub
    i32.const 17
     i32.add
   call $add
   drop
    i32.const 3
    i32.const 2
    i32.const 1
     i32.add
     i32.mul
   call $add
   drop
    i32.const 7
    i32.const 3
    i32.const 2
   call $sqr
     i32.mul
     i32.add
   call $add
   drop
    local.set $array
    i32.const 0
    local.set $sum
    i32.const 0
    local.set $i
;;loop 
    block  $00000
      loop  $00001
    local.get $i
    local.get $array
   call $size
     i32.ge_s
   if
    br  $00000
   end
    local.get $array
    local.get $i
   call $get
    local.set $x
    local.get $i
     i32.const 1
     i32.add
    local.set $i
    local.get $x
    i32.const 99
     i32.eq
   if
    br  $00000
   end
    local.get $x
    i32.const 2
     i32.le_s
   if
else
    i32.const 1
    local.set $j
;;loop 
    block  $00002
      loop  $00003
    local.get $j
     i32.const 1
     i32.add
    local.set $j
    local.get $j
    local.get $x
     i32.gt_s
   if
    br  $00002
   else
    local.get $x
    local.get $j
     i32.eq
   if
    local.get $sum
    local.get $x
     i32.add
    local.set $sum
else
    local.get $x
    local.get $j
     i32.rem_s
    i32.const 0
     i32.eq
   if
    br  $00002
end
end
   end
    br  $00003
      end
    end
   end
    br  $00001
      end
    end
    local.get $sum
    i32.const 88
     i32.eq
   if
 i32.const 0
 call $new
 local.set $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp

i32.const 84;; T
call $add
drop

i32.const 104;; h
call $add
drop

i32.const 101;; e
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 112;; p
call $add
drop

i32.const 114;; r
call $add
drop

i32.const 111;; o
call $add
drop

i32.const 103;; g
call $add
drop

i32.const 114;; r
call $add
drop

i32.const 97;; a
call $add
drop

i32.const 109;; m
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 119;; w
call $add
drop

i32.const 111;; o
call $add
drop

i32.const 114;; r
call $add
drop

i32.const 107;; k
call $add
drop

i32.const 115;; s
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 102;; f
call $add
drop

i32.const 105;; i
call $add
drop

i32.const 110;; n
call $add
drop

i32.const 101;; e
call $add
drop

i32.const 33;; !
call $add
drop

i32.const 10;; \n
call $add
drop

   call $prints
   drop
else
 i32.const 0
 call $new
 local.set $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp
 local.get $_temp

i32.const 84;; T
call $add
drop

i32.const 104;; h
call $add
drop

i32.const 105;; i
call $add
drop

i32.const 115;; s
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 112;; p
call $add
drop

i32.const 114;; r
call $add
drop

i32.const 111;; o
call $add
drop

i32.const 103;; g
call $add
drop

i32.const 114;; r
call $add
drop

i32.const 97;; a
call $add
drop

i32.const 109;; m
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 115;; s
call $add
drop

i32.const 117;; u
call $add
drop

i32.const 99;; c
call $add
drop

i32.const 107;; k
call $add
drop

i32.const 115;; s
call $add
drop

i32.const 33;; !
call $add
drop

i32.const 10;; \n
call $add
drop

   call $prints
   drop
   end
  i32.const 0
  )
)
