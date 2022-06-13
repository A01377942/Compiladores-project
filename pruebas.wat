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

  (func $reverse
  (param $array i32)
  (result i32)
   (local $_temp i32)
    (local $start i32)
    (local $finish i32)
    (local $temp i32)
    i32.const 0
    local.set $start
    local.get $array
   call $size
    local.set $finish
;;loop 
    block  $00000
      loop  $00001
    local.get $start
    local.get $finish
     i32.ge_s
   if
    local.get $start
    local.get $finish
     i32.ge_s
    br  $00000
   end
    local.get $array
    local.get $start
   call $get
    local.set $temp
    local.get $array
    local.get $start
    local.get $array
    local.get $finish
   call $get
   call $set
   drop
    local.get $array
    local.get $finish
    local.get $temp
   call $set
   drop
    local.get $start
     i32.const 1
     i32.add
    local.get $finish
     i32.const 1
     i32.sub
    br  $00001
      end
    end
   i32.const 0
   return
  )
  (func $binary
  (param $num i32)
  (result i32)
   (local $_temp i32)
    (local $result i32)
    (local $remainder i32)
    local.get $num
    i32.const 0
     i32.le_s
   if
    local.get $num
    i32.const 0
     i32.le_s
 i32.const 0
 call $new
 local.set $_temp
 local.get $_temp
 local.get $_temp

i32.const 48;; 0
call $add
drop

   return
   end
 i32.const 0
 call $new
 local.set $_temp
 local.get $_temp

    local.set $result
;;loop 
    block  $00002
      loop  $00003
    local.get $num
    i32.const 0
     i32.le_s
   if
    local.get $num
    i32.const 0
     i32.le_s
    br  $00002
   end
    local.get $num
    i32.const 2
     i32.rem_s
    local.set $remainder
    local.get $result
    local.get $remainder
   i32.const 48;; 0
     i32.add
   call $add
   drop
    local.get $num
    i32.const 2
     i32.div_s
    local.set $num
    br  $00003
      end
    end
    local.get $result
   call $reverse
   drop
    local.get $result
   return
  )
  (func
    (export "main")
    (result i32)
   (local $_temp i32)
    (local $option i32)
    (local $num i32)
;;loop 
    block  $00004
      loop  $00005
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

i32.const 73;; I
call $add
drop

i32.const 110;; n
call $add
drop

i32.const 112;; p
call $add
drop

i32.const 117;; u
call $add
drop

i32.const 116;; t
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 97;; a
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 110;; n
call $add
drop

i32.const 117;; u
call $add
drop

i32.const 109;; m
call $add
drop

i32.const 98;; b
call $add
drop

i32.const 101;; e
call $add
drop

i32.const 114;; r
call $add
drop

i32.const 58;; :
call $add
drop

i32.const 32;;  
call $add
drop

   call $prints
   drop
   call $readi
    local.set $num
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

i32.const 67;; C
call $add
drop

i32.const 111;; o
call $add
drop

i32.const 110;; n
call $add
drop

i32.const 118;; v
call $add
drop

i32.const 101;; e
call $add
drop

i32.const 114;; r
call $add
drop

i32.const 115;; s
call $add
drop

i32.const 105;; i
call $add
drop

i32.const 111;; o
call $add
drop

i32.const 110;; n
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 116;; t
call $add
drop

i32.const 111;; o
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 98;; b
call $add
drop

i32.const 105;; i
call $add
drop

i32.const 110;; n
call $add
drop

i32.const 97;; a
call $add
drop

i32.const 114;; r
call $add
drop

i32.const 121;; y
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 111;; o
call $add
drop

i32.const 102;; f
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 116;; t
call $add
drop

i32.const 104;; h
call $add
drop

i32.const 97;; a
call $add
drop

i32.const 116;; t
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 110;; n
call $add
drop

i32.const 117;; u
call $add
drop

i32.const 109;; m
call $add
drop

i32.const 98;; b
call $add
drop

i32.const 101;; e
call $add
drop

i32.const 114;; r
call $add
drop

i32.const 58;; :
call $add
drop

i32.const 32;;  
call $add
drop

   call $prints
   drop
    local.get $num
   call $binary
   call $prints
   drop
   call $println
   drop
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

i32.const 67;; C
call $add
drop

i32.const 111;; o
call $add
drop

i32.const 110;; n
call $add
drop

i32.const 118;; v
call $add
drop

i32.const 101;; e
call $add
drop

i32.const 114;; r
call $add
drop

i32.const 116;; t
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 97;; a
call $add
drop

i32.const 110;; n
call $add
drop

i32.const 111;; o
call $add
drop

i32.const 116;; t
call $add
drop

i32.const 104;; h
call $add
drop

i32.const 101;; e
call $add
drop

i32.const 114;; r
call $add
drop

i32.const 32;;  
call $add
drop

i32.const 110;; n
call $add
drop

i32.const 117;; u
call $add
drop

i32.const 109;; m
call $add
drop

i32.const 98;; b
call $add
drop

i32.const 101;; e
call $add
drop

i32.const 114;; r
call $add
drop

i32.const 63;; ?
call $add
drop

i32.const 32;;  
call $add
drop

   call $prints
   drop
   call $reads
    local.set $option
    local.get $option
   call $size
    i32.const 0
     i32.eq
   if
    local.get $option
   call $size
    i32.const 0
     i32.eq
   i32.const 78;; N
    local.set $option
   end
    br  $00005
      end
    end
  i32.const 0
  )
)
