using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using FunctionHacker.Classes;

namespace BufferOverflowProtection
{
    public static class oAssemblyCode
    {
        private static string countCall =               "push eax " +
                                                        "push ecx " +
                                                        "mov eax, [ebp+10] " +
                                                        "mov ecx, [eax]" +
                                                        "add ecx, 01000000 " +
                                                        "mov [eax], ecx " +
                                                        "pop ecx " +
                                                        "pop eax ";

        private static string isValidUnicode =        // Input: ecx as characters to check.
                                                        // Output: eax. 1 is invalid, 0 is valid
                                                        ";isValidUnicode: " +
                                                        "mov eax, 01000000 " +
                                                        "ret " +                                                     

                                                        "push ebx " +

                                                        // First character
                                                        "mov ebx, ecx " +
                                                        "and ebx, FF000000 " +
                                                        "cmp ebx, 20000000 " +
                                                        "jl <offset.invalidUnicode> " +
                                                        "cmp ebx, 7E000000 " +
                                                        "jg <offset.invalidUnicode> " +

                                                        // Second character
                                                        "mov ebx, ecx " +
                                                        "and ebx, 00FF0000 " +
                                                        "cmp ebx, 00000000 " +
                                                        "jne <offset.invalidUnicode> " +

                                                        // Third character
                                                        "mov ebx, ecx " +
                                                        "and ebx, 0000FF00 " +
                                                        "cmp ebx, 00002000 " +
                                                        "jl <offset.invalidUnicode> " +
                                                        "cmp ebx, 00007E00 " +
                                                        "jg <offset.invalidUnicode> " +

                                                        // Fourth character
                                                        "mov ebx, ecx " +
                                                        "and ebx, 000000FF " +
                                                        "cmp ebx, 00000000 " +
                                                        "jne <offset.invalidUnicode> " +

                                                        // Valid ascii string
                                                        "mov eax, 01000000 " +
                                                        "pop ebx " +
                                                        "ret " +

                                                        // Invalid ascii string
                                                        ";invalidUnicode: " +
                                                        "mov eax, 00000000 " +
                                                        "pop ebx " +
                                                        "ret "
                                                        ;

        private static string isValidAscii =            // Input: ecx as characters to check.
                                                        // Output: eax. 1 is invalid, 0 is valid
                                                        ";isValidAscii: " +
                                                        "mov eax, 01000000 " +                                                        
                                                        "ret " +

                                                        
                                                        "push ebx " +

                                                        // First character
                                                        "mov ebx, ecx " +
                                                        "and ebx, FF000000 " +
                                                        "cmp ebx, 20000000 " +
                                                        "jl <offset.invalid> " +
                                                        "cmp ebx, 7E000000 " +
                                                        "jg <offset.invalid> " +

                                                        // Second character
                                                        "mov ebx, ecx " +
                                                        "and ebx, 00FF0000 " +
                                                        "cmp ebx, 00200000 " +
                                                        "jl <offset.invalid> " +
                                                        "cmp ebx, 007E0000 " +
                                                        "jg <offset.invalid> " +

                                                        // Third character
                                                        "mov ebx, ecx " +
                                                        "and ebx, 0000FF00 " +
                                                        "cmp ebx, 00002000 " +
                                                        "jl <offset.invalid> " +
                                                        "cmp ebx, 00007E00 " +
                                                        "jg <offset.invalid> " +

                                                        // Fourth character
                                                        "mov ebx, ecx " +
                                                        "and ebx, 000000FF " +
                                                        "cmp ebx, 00000020 " +
                                                        "jl <offset.invalid> " +
                                                        "cmp ebx, 0000007E " +
                                                        "jg <offset.invalid> " +

                                                        // Valid ascii string
                                                        "mov eax, 01000000 " +                                                        
                                                        "pop ebx " +
                                                        "ret " +

                                                        // Invalid ascii string
                                                        ";invalid: " +
                                                        "mov eax, 00000000 " +                                                        
                                                        "pop ebx " +
                                                        "ret "
                                                        ;
                                                        

        private static string getDereference =          // Function definition:
                                                        // Inputs:
                                                        //    ebp + 8: pointer
                                                        //    ebp + C: argumentIndex
                                                        //    ebp + 10: dereferenceString* result
                                                        //
                                                        // Outputs:
                                                        //    eax: 1 if dereference found and structure filled out, 0 otherwise.
                                                        ";getDereference: " +                                                        
                                                        "push ebp " +
                                                        "mov ebp, esp " + 
                                                        "push ecx " +
                                                        "push edi " +
                                                        

                                                        // Check to see if the argument is a valid pointer
                                                        // by using sLOOKUPVALIDPOINTER
                                                        "mov ecx, [ebp+08] " +

                                                        // Check that the address is below 0x7fff0000
                                                        "shr ecx, 10 " + // Divide top 4 bytes
                                                        "cmp ecx, ff7f0000 " +
                                                        "jg <offset.returnNoDereference> " +
                                                        "mov ecx, [ebp+08] " +

                                                        "shr ecx, 0C " + // Divide by 0x1000
                                                        "add ecx, <sLOOKUPVALIDPOINTER> " + // Add to base of the lookup table
                                                        "mov ecx, [ecx] " + // Read in the is valid read byte flag
                                                        "and ecx, FF000000 " +
                                                        "cmp ecx, 00000000 " + 
                                                        "je <offset.returnNoDereference> " +

                                                        // Check to see if the argument + sSTRIBNGDEREFERENCESIZE is a valid pointer
                                                        "movdfr ecx, <sSTRINGDEREFERENCESIZE> " + 
                                                        "add ecx, [ebp+08] " +                       
                                                        "shr ecx, 0C " + // Divide by 0x1000
                                                        "add ecx, <sLOOKUPVALIDPOINTER> " + // Add to base of the lookup table
                                                        "mov ecx, [ecx] " + // Read in the is valid read byte flag
                                                        "and ecx, FF000000 " +
                                                        "cmp ecx, 00000000 " +
                                                        "je <offset.returnNoDereference> " +

                                                        // Check if this is a valid string.
                                                        // A valid string is defined as either:
                                                        //  1. [20 to 7E], [20 to 7E], [20 to 7E], [20 to 7E] --> ascii string
                                                        //  2. 4bytes, [20 to 7E], [20 to 7E], [20 to 7E], [20 to 7E] --> ascii string with length prefix
                                                        //  3. [20 to 7E], 0, [20 to 7E], 0 --> unicode string
                                                        //  4. 4bytes, [20 to 7E], 0, [20 to 7E], 0 --> unicode string with length prefix

                                                        // Read the first four bytes
                                                        "mov ecx, [ebp+08] " +
                                                        "mov ecx, [ecx] " +

                                                        // 1. Check if it is valid ascii
                                                        "call <offset.isValidAscii>" +  
                                                        "cmp eax, 01000000 " +
                                                        "je <offset.validDereference> " +

                                                        // 3. Check if valid unicode
                                                        "call <offset.isValidUnicode>" +
                                                        "cmp eax, 01000000 " +
                                                        "je <offset.validDereference> " +

                                                        // Read in the next four bytes
                                                        "mov ecx, [ebp+08] " +
                                                        "add ecx, 04000000 " +                                                        
                                                        "mov ecx, [ecx] " +

                                                        // 2. Check if it is valid ascii with data prefix
                                                        "call <offset.isValidAscii>" +
                                                        "cmp eax, 01000000 " +
                                                        "je <offset.validDereference> " +

                                                        // 4. Check if valid unicode with data prefix
                                                        "call <offset.isValidUnicode>" +
                                                        "cmp eax, 01000000 " +
                                                        "je <offset.validDereference> " +

                                                        // Return with no dereferences
                                                        ";returnNoDereference: " +
                                                        "mov eax, 00000000 " +
                                                        "pop edi " +
                                                        "pop ecx " +
                                                        "pop ebp " +
                                                        "C2 0C00" +

                                                        // Valid dereference
                                                        ";validDereference: " +
                                                        // struct dereferenceString{
                                                        //   uint stringAddress;
                                                        //   uint argumentIndex; // indicates the corresponding argument. 0 -> ecx, 1 -> edx, 2 -> param1, 3 -> param2, etc.
                                                        //   char[*sSTRINGDEREFERENCESIZE] string;
                                                        // };

                                                        "push esi " +
                                                        "mov edi, [ebp+10] " + // structure location
                                                        "mov esi, [ebp+08]  " + // pointer
                                                        "mov [edi], esi " + // save pointer
                                                        "mov ecx, [ebp+0C] " + // arg index
                                                        "add edi, 04000000 " +
                                                        "mov [edi], ecx " + // save arg index
                                                        "add edi, 04000000 " +
                                                        "movdfr ecx, <sSTRINGDEREFERENCESIZE> " +                                                      

                                                        // Copy dereferenced string
                                                        "rep movs byte [edi], byte [esi] " + // ecx is the count.
                                                        
                                                        "mov eax, 01000000 " +
                                                        "pop esi " +
                                                        "pop edi " +
                                                        "pop ecx " +
                                                        "pop ebp " +
                                                        "C2 0C00";

        public static string addStringDereferencesToStack =  
                                                    // Load the string dereferences
                                                    // Local variables:     esi -> struct size
                                                    //                      ecx -> number of strings to dereference
                                                    //                      edx -> number of stack args left to dereference
                                                    //                      edi -> stack pointer to current stack argument
                                                    //                      
                                                    //
                                                    // ebp + 28 -> stack arg3
                                                    // ebp + 24 -> stack arg2
                                                    // ebp + 20 -> stack arg1
                                                    // ebp + 1C -> source address
                                                    // ebp + 18 -> useInvalidSourceTable
                                                    // ebp + 14 -> number of arguments
                                                    // ebp + 10 -> function count address
                                                    // ebp + 0C -> record detailed information count
                                                    // ebp + 08 -> destination
                                                    // ebp - 04 -> original ecx
                                                    // ebp - 08 -> original edx
                                                    // ebp - 0C -> original esp
                                                    // ebp - 10 -> original eax

                                                    // Calculate a struct size
                                                    "movdfr esi, <sSTRINGDEREFERENCESIZE> " +
                                                    "add esi, 08000000 " + // Space for stringAddress, argumentIndex
                                                    "sub esp, esi " + // Space for a dereferenced string struct

                                                    // --- CHECK argument ECX
                                                    "push esp " + // dereference string result
                                                    "push 00000000 " + // argument index
                                                    "mov ecx, [ebp-04] " +
                                                    "push ecx " + // pointer to potential string
                                                    "call <offset.getDereference> " +

                                                    "cmp eax, 00000000 " +
                                                    "jz <offset.noEsiDereference> " +
                                                    "sub esp, esi " + // Add another structure to the stack
                                                    ";noEsiDereference: " +
                                                    "mov ecx, eax " + // Set the number of string dereferences found

                                                    // --- CHECK argument EDX
                                                    "push esp " + // dereference string result
                                                    "push 01000000 " + // argument index
                                                    "mov edx, [ebp-08] " +
                                                    "push edx " + // pointer to potential string
                                                    "call <offset.getDereference> " +

                                                    "cmp eax, 00000000 " +
                                                    "jz <offset.noEdxDereference> " +
                                                    "sub esp, esi " + // Add another structure to the stack
                                                    ";noEdxDereference: " +
                                                    "add ecx, eax " + // Set the number of string dereferences found

                                                    // --- CHECK argument EAX
                                                    "push esp " + // dereference string result
                                                    "push 02000000 " + // argument index
                                                    "mov edx, [ebp-10] " +
                                                    "push edx " + // pointer to potential string
                                                    "call <offset.getDereference> " +

                                                    "cmp eax, 00000000 " +
                                                    "jz <offset.noEaxDereference> " +
                                                    "sub esp, esi " + // Add another structure to the stack
                                                    ";noEaxDereference: " +
                                                    "add ecx, eax " + // Set the number of string dereferences found


                                                    // --- CHECK STACK arguments
                                                    "mov edx, [ebp+14] " + // Load number of parameters

                                                    // Load the last argument pointer
                                                    "mov edi, edx " +
                                                    "sub edi, 01000000 " +
                                                    "shl edi, 02 " + 
                                                    "add edi, ebp " + 
                                                    "add edi, 20000000 " +

                                                    ";nextArgDereference: " +
                                                    "cmp edx, 00000000" +
                                                    "je <offset.finishedDereferencing> " +

                                                    // --- Dereference this stack argument
                                                    "push esp " + // dereference string result
                                                    "mov eax, 03000000 " +
                                                    "add eax, edx " +
                                                    "push eax " + // argument index
                                                    "push [edi] " + // pointer to potential string
                                                    "call <offset.getDereference> " +

                                                    "cmp eax, 00000000 " +
                                                    "jz <offset.noStackDereference> " +
                                                    "sub esp, esi " + // Add another structure to the stack
                                                    ";noStackDereference: " +
                                                    "add ecx, eax " + // Set the number of string dereferences found

                                                    // Move on to the next stack argument
                                                    "dec edx " +
                                                    "sub edi, 04000000 " +
                                                    "jmp <offset.nextArgDereference> " +                                                    


                                                    // --- Finished dereferencing all the arguments
                                                    ";finishedDereferencing: " +
                                                    // Clean up the top-most empty dereference struct
                                                    "add esp, esi " +
                                                    // Set the return value as the number of structs added to the stack
                                                    "mov eax, ecx " +                                                    
                                                    "";
                                    
        public static string recordVisualizationInfo = 
                                                    // First load the circular buffer address, and increment it by 8.
                                                    // With the lock xchange add instruction there should exist no
                                                    // race condition if I understand it correctly.

                                                    // Circular buffer call entry struct format:
                                                    // 
                                                    // struct callRecordedData{
                                                    //   uint destination;
                                                    //   uint source;
                                                    //   uint esp;  - for determining thread of caller
                                                    //   uint ecx;  - __fastcall argument 1, __thiscall 'this' pointer
                                                    //   uint edx;  - __fastcall argument 2
                                                    //   uint eax;  - __fastcall argument 3 on borland
                                                    //   uint numParams; - stack arguments
                                                    //   uint[numParams] params; - stack arguments
                                                    //   uint numStrings;
                                                    //   dereferenceString[numStrings] strings;
                                                    // };
                                                    // 
                                                    // struct dereferenceString{
                                                    //   uint stringAddress;
                                                    //   uint argumentIndex; // indicates the corresponding argument. 0 -> ecx, 1 -> edx, 2 -> param1, 3 -> param2, etc.
                                                    //   char[*sSTRINGDEREFERENCESIZE] string;
                                                    // };
                                                    //
                                                    // ...      -> stack arg...
                                                    // ebp + 28 -> stack arg3
                                                    // ebp + 24 -> stack arg2
                                                    // ebp + 20 -> stack arg1
                                                    // ebp + 1C -> source address
                                                    // ebp + 18 -> useInvalidSourceTable
                                                    // ebp + 14 -> number of arguments
                                                    // ebp + 10 -> function count address
                                                    // ebp + 0C -> record detailed information count
                                                    // ebp + 08 -> destination
                                                    // ebp      -> original ecx
                                                    // ebp - 04 -> original ecx
                                                    // ebp - 08 -> original edx
                                                    // ebp - 0C -> original esp
                                                    // ebp - 10 -> original eax

                                                    // --- Load the dereferences onto the stack
                                                    addStringDereferencesToStack +
                                                    "mov edi, eax " +

                                                    // --- Reserve space in the circular buffer
                                                    ";restartDataStorage: " +                                                    

                                                    // Calculate the size of the dereferences
                                                    "movdfr esi, <sSTRINGDEREFERENCESIZE> " +
                                                    "add esi, 08000000 " + // Space for stringAddress, argumentIndex
                                                    "mul esi " + // multiply, edx..EDX:EAX = esi * eax.
                                                    "mov edx, eax " +                                                    

                                                    "mov eax, <VIS_AD_CIRCULAR_OFFSET> " +
                                                    "mov ecx, [ebp+14] " + // Load number of parameters
                                                    "shl ecx, 02" + // Shift left by 2, because we need 4 bytes per parameter
                                                    "add ecx, 20000000 " + // Space for destination, source, esp, ecx, edx, eax, numParams, numStrings
                                                    "add ecx, edx " + // Space for all the dereferences
                                                    "lock xadd [eax], ecx " + // Reserve the space

                                                    // Check if our circular buffer has hit the end of the buffer
                                                    "cmp ecx, <VIS_CIRCULAR_SIZE> " +
                                                    "jl <offset.skipWrapBuffer> " +

                                                    // Wrap the circular buffer offset
                                                    "mov eax, <VIS_AD_CIRCULAR_OFFSET> " +
                                                    "mov [eax], 00000000 " +
                                                    "mov eax, edi " +

                                                    // Restart looking for a place to store this data
                                                    "jmp <offset.restartDataStorage> " +                                                    

                                                    ";skipWrapBuffer: " +

                                                    // --- Save this callRecordedData structure

                                                    // - desination
                                                    "mov eax, <VIS_AD_CIRCULAR_BASE> " +
                                                    "add eax, ecx " +
                                                    "mov ecx, [ebp+08] " +
                                                    "mov [eax], ecx " +

                                                    // - source
                                                    "add eax, 04000000 " +
                                                    "mov ecx, [ebp+1C] " +
                                                    "mov [eax], ecx " +

                                                    // - esp
                                                    "add eax, 04000000 " +
                                                    "mov ecx, [ebp-0C] " +
                                                    "mov [eax], ecx " +

                                                    // - ecx
                                                    "add eax, 04000000 " +
                                                    "mov ecx, [ebp-04] " +
                                                    "mov [eax], ecx " +

                                                    // - edx
                                                    "add eax, 04000000 " +
                                                    "mov ecx, [ebp-08] " +
                                                    "mov [eax], ecx " +

                                                    // - eax
                                                    "add eax, 04000000 " +
                                                    "mov ecx, [ebp-10] " +
                                                    "mov [eax], ecx " +

                                                    // - numParams
                                                    "add eax, 04000000 " +
                                                    "mov ecx, [ebp+14] " + // Save the number of parameters
                                                    "mov [eax], ecx " +

                                                    // - params
                                                    "mov esi, [ebp+14] " + // Load number of parameters
                                                    "mov ecx, ebp " +
                                                    "add ecx, 1C000000 " +
                                                    ";saveArgLoopStart: " +
                                                    "cmp esi, 00000000 " +
                                                    "jle <offset.saveArgLoopExit> " +
                                                    // Save this argument
                                                    "add ecx, 04000000 " +
                                                    "add eax, 04000000 " +
                                                    "mov edx, [ecx] " +
                                                    "mov [eax], edx " +
                                                    // Return to loop start
                                                    "dec esi " +
                                                    "jmp <offset.saveArgLoopStart> " +
                                                    ";saveArgLoopExit: " +

                                                    // - numStrings
                                                    "add eax, 04000000 " +
                                                    "mov [eax], edi " + // edi -> num of dereferences

                                                    // - strings
                                                    "add eax, 04000000 " +
                                                    "mov ecx, eax " +

                                                    // Load the size of the strings
                                                    "movdfr esi, <sSTRINGDEREFERENCESIZE> " +
                                                    "add esi, 08000000 " + // Space for stringAddress, argumentIndex
                                                    "mov eax, edi " + // edi -> num strings
                                                    "mul esi " + // multiply, EDX:EAX = esi * eax.
                                                    "mov edx, eax " +    

                                                    // Copy the strings to the circular buffer
                                                    "mov esi, esp " + // set source
                                                    "mov edi, ecx " +
                                                    "mov ecx, edx " + // length
                                                    "push ecx " + 
                                                    "rep movs byte [edi], byte [esi] " + // ecx is the count.
                                                    "pop ecx " +

                                                    // Remove the string dereferences from the stack
                                                    "add esp, ecx "
                                                    ;

        public static string mainRecordFunction = oAssemblyGenerator.processLabels(oAssemblyGenerator.replaceCommands(
                                                    // This is setup as a common function so that the processor caching should
                                                    // keep this in a very high level cache, making it more efficient.

                                                    // (Call Source Arguments, Call Source Address)
                                                    //          ebp + 1C           ebp + 18

                                                    // Inputs arguments:
                                                    // ...
                                                    // ebp + 28 -> stack arg3
                                                    // ebp + 24 -> stack arg2
                                                    // ebp + 20 -> stack arg1
                                                    // ebp + 1C -> source address
                                                    // ebp + 18 -> useInvalidSourceTable
                                                    // ebp + 14 -> number of arguments
                                                    // ebp + 10 -> function count address
                                                    // ebp + 0C -> record detailed information count
                                                    // ebp + 08 -> destination
                                                    // ebp - 04 -> original ecx
                                                    // ebp - 08 -> original edx
                                                    // ebp - 0C -> original esp
                                                    // ebp - 10 -> original eax

                                                    // Setup our base pointer, so we can easily access the function arguments on the stack.
                                                    "push ebp " +
                                                    "mov ebp, esp " +
                                                    "push ecx " + // ebp - 4
                                                    "push edx " + // ebp - 8
                                                    "push esp " + // ebp - C
                                                    "push eax " + // ebp - 10

                                                    // Save the registers and flags, we have to leave everything the same.
                                                    "pushfd " +
                                                    "pushad " +

                                                    // Check if this is an inter-modular call. If so we will check if the call source is excluded.
                                                    "mov ecx, [ebp+18] " +
                                                    "cmp ecx, 00000000 " +
                                                    "jz <offset.skipCheckSource> " +

                                                        // Check if the source address is excluded
                                                        "mov ecx, [ebp+1C] " + // ecx = calling address
                                                        "shr ecx, 0C " + // Divide by 0x1000
                                                        "add ecx, <sLOOKUPINVALIDSOURCE> " + // Add to base of the lookup table
                                                        "mov ecx, [ecx] " + // Read in the is valid read byte flag
                                                        "and ecx, FF000000 " +
                                                        "cmp ecx, 00000000 " +
                                                        "jnz <offset.skipRecordData> " +

                                                    ";skipCheckSource: " +


                                                    // Count this function call
                                                    countCall +


                                                    // Check that we are recording call parameter information
                                                    "mov eax, [ebp+0C] " +                                                    
                                                    "mov eax, [eax] " +
                                                    "cmp eax, 00000000 " +
                                                    "je <offset.skipRecordData> " +

                                                        // If our function call count has exceeded global parameter 
                                                        // MAXCALLS, we set sSAVEDATA = 0. This is to prevent a
                                                        // super-commonly called function from filling up the buffer
                                                        // quickly.
                                                        "mov esi, [ebp+10] " +
                                                        "mov esi, [esi] " +
                                                        "movdfr eax, <MAXCALLS> " +
                                                        "cmp eax, esi " +
                                                        "jl <offset.skipRecordData> " +

                                                    // Now record the call visualization information
                                                    recordVisualizationInfo +

                                                    ";skipRecordData:" +
                                                    "popad " +
                                                    "popfd " +
                                                    "pop eax " +
                                                    "pop esp " +
                                                    "pop edx " +
                                                    "pop ecx " +
                                                    "pop ebp " +

                                                    // Return and pop 0x14 bytes
                                                    "C2 1400" +

                                                    // Functions used by this code
                                                    isValidUnicode +
                                                    isValidAscii +
                                                    getDereference +

                                                    ";sSTRINGDEREFERENCESIZE: " +
                                                    "(4) " + // Set to 128

                                                    ";sLOOKUPVALIDPOINTER: " +
                                                    "(524288) "+// 0x80000 bytes. Each byte represents whether the corresponding
                                                                // block is valid as a pointer destination. Byte 0 corresponds to
                                                                // block from 0x0 to 0x999, Byte 1 to 0x1000 to 0x1999 etc.

                                                    ";sLOOKUPINVALIDSOURCE: " +
                                                    "(524288) " // 0x80000 bytes. Each byte represents whether the corresponding
                                                                // block is invalid as a call source. Byte 0 corresponds to
                                                                // block from 0x0 to 0x999, Byte 1 to 0x1000 to 0x1999 etc.
                                                                // 1 means invalid call source, 0 means valid.*/
                                                    )
                                                    );


        public static string mainInjection = oAssemblyGenerator.replaceCommands(
                                                    // Push the important variables to the stack as arguments for the common function
                                                    //"push esp " +

                                                    "push <useInvalidSourceTable>" +
                                                    "push <numParameters> " +
                                                    "push <sCOUNT> " +
                                                    "push <sSAVEDATA> " +
                                                    "push <destination> " +                                                    

                                                    // Call the main processing function
                                                    "call <offset.mainRecordFunction> " + 
                                                    
                                                    // Resume with the function call
                                                    "jmp <offset.destination> " +

                                                    // Allocate the storage space
                                                    ";useInvalidSourceTable:" +
                                                    "(4)" +
                                                    ";sCOUNT:" +
                                                    "(4)" +
                                                    ";sSAVEDATA:" +
                                                    "(4)"
                                                    );


        public static string mainInjectionEndpoint = oAssemblyGenerator.replaceCommands(
                                                    // Push the important variables to the stack as arguments for the common function
                                                    //"push esp " +                                                  
                                                    "push <numParameters> " +
                                                    "push <sCOUNT> " +
                                                    "push <sSAVEDATA> " +
                                                    "push <destination> " +

                                                    // Call the main processing function
                                                    "call <offset.mainRecordFunction> " +

                                                    // Execute the original replaced code
                                                    "<replacedCode>" +

                                                    // Resume with the function call
                                                    "jmp <offset.destination> " +

                                                    // Allocate the storage space
                                                    ";sCOUNT:" +
                                                    "(4)" +
                                                    ";sSAVEDATA:" +
                                                    "(4)"
                                                    );
    }

    public enum CommonFlag { None = 0, Fixed };
    public class CommonVariable
    {
        public string value;
        public CommonFlag flag;

        public CommonVariable(string value, CommonFlag flag)
        {
            this.value = value;
            this.flag = flag;
        }
    }

    public static class oAssemblyGenerator
    {
        private static Hashtable commonAddresses = new Hashtable(0);

        /// <summary>
        /// This function processes the labels in the code, adding them to the
        /// common address table and removing them from the code.
        /// </summary>
        /// <param name="code"></param>
        public static string processLabels(string code)
        {
            // First perform the pre-processing by removing the variables
            string codeNoVars = code.Replace(" ","");
            int index = 0;
            while ((index = codeNoVars.IndexOf("<",index)) != -1)
            {
                // Replace this occurance with gggggggg
                int indexEnd = codeNoVars.IndexOf(">",index);
                codeNoVars = codeNoVars.Remove(index, indexEnd - index + 1);
                codeNoVars = codeNoVars.Insert(index, "gggggggg");
            }

            // Now we can accurately calculate the offsets for each label
            index = 0;
            while ((index = codeNoVars.IndexOf(";",index)) != -1)
            {
                // Load the label name and remove this label
                int indexEnd = codeNoVars.IndexOf(":",index);
                string label = codeNoVars.Substring(index + 1, indexEnd - index - 1);
                codeNoVars = codeNoVars.Remove(index, indexEnd - index + 1);
                
                // Add the common address
                addCommonAddress((uint)(index/2), label, CommonFlag.Fixed);
            }

            // Validate the resulting length of code
            if ((codeNoVars.Length % 2) > 0)
            {
                MessageBox.Show("Error: Length of code injection is odd after processing labels.");
            }

            // Return the string with the labels removed
            string codeNoLabels = code;
            index = 0;
            while ((index = codeNoLabels.IndexOf(";", index)) != -1)
            {
                // Remove the label
                int indexEnd = codeNoLabels.IndexOf(":", index);
                codeNoLabels = codeNoLabels.Remove(index, indexEnd - index + 1);
            }

            return codeNoLabels;
        }

        public static void printCommonAddresses()
        {
            oConsole.printMessage("Common Addresses:");

            // Loop through all items of the Hashtable
            IDictionaryEnumerator en = commonAddresses.GetEnumerator();
            while (en.MoveNext())
            {
                oConsole.printMessage(((string)en.Key) + " = " + ((CommonVariable)en.Value).value);
            }
        }

        public static void addCommonAddressNoRev(uint address, string name)
        {
            addCommonAddressNoRev(address, name, CommonFlag.None);
        }

        public static void addCommonAddressNoRev(uint address, string name, CommonFlag flag)
        {
            // Convert the address to a hex string with length 8
            string addressHex = address.ToString("X");
            while (addressHex.Length < 8)
                addressHex = "0" + addressHex;

            // Reverse the bytes
            string addressHexRev = addressHex.Substring(0, 2) + addressHex.Substring(2, 2) + addressHex.Substring(4, 2) + addressHex.Substring(6, 2);

            addCommonAddress(addressHexRev, name, flag);
        }

        public static void addCommonAddress(uint address, string name)
        {
            addCommonAddress(address, name, CommonFlag.None);
        }

        public static void addCommonAddress(uint address, string name, CommonFlag flag)
        {
            // Convert the address to a hex string with length 8
            string addressHex = address.ToString("X");
            while (addressHex.Length < 8)
                addressHex = "0" + addressHex;

            // Reverse the bytes
            string addressHexRev = addressHex.Substring(6, 2) + addressHex.Substring(4, 2) + addressHex.Substring(2, 2) + addressHex.Substring(0, 2);

            addCommonAddress(addressHexRev, name, flag);
        }

        public static void addCommonAddress(string newCommonAddress, string commonAddressName)
        {
            addCommonAddress(newCommonAddress, commonAddressName, CommonFlag.None);
        }

        public static void addCommonAddress(string newCommonAddress, string commonAddressName, CommonFlag flag)
        {
            // Check if this common address exists yet
            if (commonAddresses.Contains(commonAddressName))
            {
                // Update the existing entry
                ((CommonVariable)commonAddresses[commonAddressName]).value = newCommonAddress;
                ((CommonVariable)commonAddresses[commonAddressName]).flag = flag;
            }
            else
            {
                // Add this common address
                commonAddresses.Add(commonAddressName, new CommonVariable(newCommonAddress,flag));
            }
        }

        public static string evaluateCommonAddress(string commonAddressName, uint commonAddressBase, uint injectionBase)
        {
            // Check if it is an offset
            bool offset = false;
            if (commonAddressName.StartsWith("offset."))
            {
                offset = true;
                commonAddressName = commonAddressName.Remove(0,7);
            }

            // Search for the command address directly
            string result = "";
            if (commonAddresses.Contains(commonAddressName))
            {
                result = ((CommonVariable) commonAddresses[commonAddressName]).value;

                // Process the fixed flag if required
                if (((CommonVariable)commonAddresses[commonAddressName]).flag == CommonFlag.Fixed)
                {
                    result = (injectionBase + uint.Parse(oMemoryFunctions.ReverseString(result), NumberStyles.AllowHexSpecifier)).ToString("X");
                    while (result.Length < 8)
                    {
                        result = "0" + result;
                    }
                    result = oMemoryFunctions.ReverseString(result);
                }
            }
            else
            {
                // If the common address was not found directly, check if it is a dll load reference
                if (commonAddressName.ToLower().StartsWith("dll."))
                {
                    // Load the dll function into the common address list

                    // Load the dll name and procedure
                    string dllName = commonAddressName.Split('.')[1];
                    string procedureName = commonAddressName.Split('.')[2];

                    // Search for the function address
                    UInt64 addressDllFunction = oMemoryFunctions.LoadAddress(dllName, procedureName, oProcess.activeProcess);

                    if (addressDllFunction != 0)
                    {
                        // Load the address of the dll procedure
                        string value = addressDllFunction.ToString("X");
                        while (value.Length < 8)
                            value = "0" + value;
                        addCommonAddress(reverseDWord(value), commonAddressName);
                        result = reverseDWord(value);
                    }
                    else
                    {
                        oConsole.printMessageShow("Failed to find procedure " + procedureName + " in dll " + dllName + ".");
                        return null;
                    }
                }
                else
                {
                    oConsole.printMessageShow("Failed to find common address variable named " + commonAddressName + ".");
                }
            }
            
            // Process the offset alteration to the result if required
            if (offset)
            {
                result = buildOffsetString(commonAddressBase - 1, UInt64.Parse(oMemoryFunctions.ReverseString(result), NumberStyles.AllowHexSpecifier));
            }
            
            // Error checking
            if (result.Length != 8 )
            {
                oConsole.printMessageShow("Variable " + commonAddressName + " with value of " + result + " should have a length of 8.");
            }
            return result;
        }

        public static void reset()
        {
            commonAddresses = new Hashtable();
        }

        /// <summary>
        /// This function builds the string injection into a byte array, performing the required replacements.
        /// </summary>
        /// <param name="sourceAddress"></param>
        /// <param name="destAddress"></param>
        /// <returns></returns>
        public static byte[] buildInjection(UInt64 injectionBase, string injectionString, Process process, uint destination, uint sourceAddress)
        {
            string tmpString = injectionString.Replace(" ", "");

            int index = 0;
            // Replace the parameters
            while ( (index = tmpString.IndexOf('<',index)) != -1)
            {
                int indexEnd = tmpString.IndexOf('>', index);
                // Load this parameter name
                string parameter = tmpString.Substring(index, indexEnd - index + 1);

                // Load the value of the parameter
                string value = "";
                switch (parameter)
                {
                    case "<destination>":
                        value = destination.ToString("X");
                        while(value.Length < 8)
                        {
                            value = "0" + value;
                        }
                        value = oMemoryFunctions.ReverseString(value);
                        break;
                    case "<offset.destination>":
                        value = buildOffsetString(injectionBase + (UInt64) (index / 2) - 1, destination);
                        break;
                    case "<offset.return>":
                        value = buildOffsetString(injectionBase + (UInt64)(index / 2) - 1, sourceAddress + 5);
                        break;
                    default:
                        // Replace it with a common variable
                        string variableName = parameter.Replace("<", "").Replace(">", "");
                        value = evaluateCommonAddress(variableName, (uint)injectionBase + (uint)(index / 2), (uint) injectionBase);                        
                        break;
                }

                // Remove the parameter
                tmpString = tmpString.Remove(index, indexEnd - index + 1);

                // Add the value of the parameter
                tmpString = tmpString.Insert(index, value);
            }


            //oConsole.printMessage("Built injection for base " + injectionBase.ToString("X") + " with code: " + tmpString);

            // Convert the string into a byte array
            byte[] result = new byte[tmpString.Length / 2];
            for (int i = 0; i < tmpString.Length; i += 2)
            {
                result[i/2] = stringToByte(tmpString[i].ToString() + tmpString[i + 1].ToString());
            }

            return result;
        }


        /// <summary>
        /// This function builds the string injection into a byte array, performing the required replacements.
        /// </summary>
        /// <param name="sourceAddress"></param>
        /// <param name="destAddress"></param>
        /// <returns></returns>
        public static string buildInjectionStringOnly(UInt64 injectionBase, string injectionString, Process process, uint destination, uint sourceAddress)
        {
            string tmpString = injectionString.Replace(" ", "");

            int index = 0;
            // Replace the parameters
            while ((index = tmpString.IndexOf('<', index)) != -1)
            {
                int indexEnd = tmpString.IndexOf('>', index);
                // Load this parameter name
                string parameter = tmpString.Substring(index, indexEnd - index + 1);

                // Load the value of the parameter
                string value = "";
                switch (parameter)
                {
                    case "<destination>":
                        value = destination.ToString("X");
                        while (value.Length < 8)
                        {
                            value = "0" + value;
                        }
                        value = oMemoryFunctions.ReverseString(value);
                        break;
                    case "<offset.destination>":
                        value = buildOffsetString(injectionBase + (UInt64)(index / 2) - 1, destination);
                        break;
                    case "<offset.return>":
                        value = buildOffsetString(injectionBase + (UInt64)(index / 2) - 1, sourceAddress + 5);
                        break;
                    default:
                        // Replace it with a common variable
                        string variableName = parameter.Replace("<", "").Replace(">", "");
                        value = evaluateCommonAddress(variableName, (uint)injectionBase + (uint)(index / 2), (uint)injectionBase);
                        break;
                }

                // Remove the parameter
                tmpString = tmpString.Remove(index, indexEnd - index + 1);

                // Add the value of the parameter
                tmpString = tmpString.Insert(index, value);
            }

            return tmpString;
        }

        

        /// <summary>
        /// This function generates a 4 byte array containing the offset between the two addresses:
        /// dest - source + 5
        /// Then the result is stored in a reverse byte array for easy injection.
        /// </summary>
        /// <param name="sourceAddress"></param>
        /// <param name="destAddress"></param>
        /// <returns></returns>
        public static byte[] buildOffset(UInt64 sourceAddress, UInt64 destAddress)
        {
            UInt64 offset = destAddress - (sourceAddress + 5);
            byte[] result = new byte[4]; ;
            result[0] = (byte)(offset & 0x000000ff);
            result[1] = (byte)((offset & 0x0000ff00) >> 8);
            result[2] = (byte)((offset & 0x00ff0000) >> 16);
            result[3] = (byte)((offset & 0xff000000) >> 24);
            return result;
        }

        /// <summary>

        /// </summary>
        /// <param name="sourceAddress"></param>
        /// <param name="destAddress"></param>
        /// <returns></returns>
        public static string reverseDWord(string dword)
        {
            string char3 = dword.Substring(0, 2);
            string char2 = dword.Substring(2, 2);
            string char1 = dword.Substring(4, 2);
            string char0 = dword.Substring(6, 2);

            return char0 + char1 + char2 + char3;
        }

        /// <summary>
        /// This function generates a 4 byte string array containing the offset between the two addresses:
        /// dest - source + 5
        /// Then the result is stored in a reverse byte array for easy injection.
        /// </summary>
        /// <param name="sourceAddress"></param>
        /// <param name="destAddress"></param>
        /// <returns></returns>
        public static string buildOffsetString(UInt64 sourceAddress, UInt64 destAddress)
        {
            UInt64 offset = destAddress - (sourceAddress + 5);
            byte[] result = BitConverter.GetBytes(offset);
            
            string resultString = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                resultString += result[i].ToString("X2");
            }
            if (resultString.Length != 8)
                oConsole.printMessageShow("Error: Failed to prepare offset string. Rusult length of " + resultString.Length.ToString() + " when expected a length of 8.");
            return resultString;
        }

        

        public static string replaceCommands(string assembly)
        {
            // Generate the instruction list
            string[,] instructions = new string[,]
            {
                {"int3", "CC"},
                {"rep movs byte [edi], byte [esi]", "F3 A4"},
                {"mov eax, [ebp+08]", "8B 45 08"},
                {"mov eax, [ebp+0C]", "8B 45 0C"},
                {"mov eax, [ebp+10]", "8B 45 10"},
                {"mov eax, [ebp+14]", "8B 45 14"},
                {"mov eax, [ebp+18]", "8B 45 18"},
                {"mov eax, [ebp+1C]", "8B 45 1C"},
                {"mov ecx, [ebp-0C]", "8B 4D F4"},
                {"mov ecx, [ebp-08]", "8B 4D F8"},
                {"mov ecx, [ebp-04]", "8B 4D FC"},
                {"mov ecx, [ebp]", "8B 4D 00"},
                {"mov ecx, [ebp+08]", "8B 4D 08"},
                {"mov ecx, [ebp+0C]", "8B 4D 0C"},
                {"mov ecx, [ebp+10]", "8B 4D 10"},
                {"mov ecx, [ebp-10]", "8B 4D F0"},
                {"mov ecx, [ebp+14]", "8B 4D 14"},
                {"mov ecx, [ebp+18]", "8B 4D 18"},
                {"mov ecx, [ebp+1C]", "8B 4D 1C"},
                {"mov esi, [ebp+08]", "8B 75 08"},
                {"mov esi, [ebp+0C]", "8B 75 0C"},
                {"mov esi, [ebp+10]", "8B 75 10"},
                {"mov esi, [ebp+14]", "8B 75 14"},
                {"mov esi, [ebp+18]", "8B 75 18"},
                {"mov esi, [ebp+1C]", "8B 75 1C"},
                {"mov edx, [ebp+14]", "8B 55 14"},
                {"mov edx, [ebp-04]", "8B 55 FC"},
                {"mov edx, [ebp-08]", "8B 55 F8"},
                {"mov edx, [ebp-10]", "8B 55 F0"},
                {"add ecx, [ebp+08]", "03 4D 08"},
                {"mov edi, [ebp+10]", "8B 7D 10"},
                {"verr cx","0F 00 E1"},
                {"verw cx", "0F 00 E9"},
                {"verr bp", "0F 00 E5"},
                {"verr di", "0F 00 E7"},
                {"verr [ecx]","0F 00 21"},
                {"fclex ", " DB E2"},
                {"push fs:[0]","64 FF 35 00 00 00 00"},
                {"mov fs:[0], eax","64 A3 00 00 00 00"},
                {"pop fs:[0]","64 8F 05 00 00 00 00"},
                {"mov fs:[0], esp","64 89 25 00 00 00 00"},
                {"push [edi]","FF 37"},
                {"push [ebp+18]","FF 75 18"},
                {"push [ebp+14]","FF 75 14"},
                {"push [ebp+10]","FF 75 10"},
                {"push [ebp+0c]","FF 75 0C"},
                {"push [ebp+08]","FF 75 08"},
                {"push [ebp+04]","FF 75 04"},
                {"push eax", "50"},
                {"push ecx", "51"},
                {"push edx", "52"},
                {"push ebx", "53"},
                {"push esp", "54"},
                {"push ebp", "55"},
                {"push esi", "56"},
                {"push edi", "57"},
                {"pop eax", "58"},
                {"pop ecx", "59"},
                {"pop edx", "5A"},
                {"pop ebx", "5B"},
                {"pop esp", "5C"},
                {"pop ebp", "5D"},
                {"pop esi", "5E"},
                {"pop edi", "5F"},
                {"pushfd", "9C"},
                {"popfd", "9D"},
                {"pushad", "60"},
                {"popad", "61"},
                {"push", "68"},
                {"call edi", "FF D7"},
                {"call ", "E8"},
                {"jmp ", "E9"},
                {"jz ", "0F 84"},
                {"jnz ", "0F 85"},
                {"jle ", "0F 8E"},
                {"shr ebp, ", "C1 ED"},
                {"mov ecx, [ecx]", "8B 09"},
                {"mov eax, [ecx]", "8B 01 "},
                {"mov edx, [ecx]", "8B 11 "},
                {"mov ecx, [eax]","8B 08"},
                {"mov ecx, esi","8B CE"},
                {"mov eax, [esp]", "8B 04 24"},
                {"mov eax, [eax]", "8B 00"},
                {"mov esi, [esi]", "8B 36"},
                {"mov [eax], ecx", "89 08"},
                {"mov [eax], esp", "89 20"},
                {"mov [esp], eax", "89 04 24"},
                {"mov [ecx], eax", "89 01"},
                {"mov [eax], edx", "89 10"},
                {"mov [eax], edi", "89 38"},
                {"mov [edi], esi", "89 37"},
                {"mov [edi], ecx","89 0F"},
                {"mov esi, ecx", "8B F1"},
                {"mov esi, esp", "8B F4"},
                {"mov edi, eax", "8B F8"},
                {"mov eax, esp", "8B C4"},
                {"mov eax, edi", "8B C7"},
                {"mov ecx, eax", "8B C8"},
                {"mov ecx, esp", "8B CC"},
                {"mov ecx, ebp", "8B CD"},
                {"mov edi, ecx", "8B F9"},
                {"mov eax, ecx", "8B C1"},
                {"mov ebp, ecx", "8B E9"},
                {"mov ebp, esp", "8B EC"},
                {"mov edx, ecx", "8B D1"},
                {"mov ecx, edx", "8B CA"},
                {"mov eax, edx", "8B C2"},
                {"mov ebx, ecx", "8B D9"},
                {"mov eax, ecx", "8B C1"},
                {"mov edi, edx", "8B FA"},
                {"mov edx, eax", "8B D0"},
                {"mov esp, ebp","8B E5"},
                {"mov esi,[ebp+10]","8B 75 10"},
                {"mov edx,[ebp+0c]","8B 55 0C"},
                {"mov [esi+c4], edx","89 96 C4 00 00 00"},
                {"mov eax, [edx+08]","8B 42 08"},
                {"mov [esi+b8], eax","89 86 B8 00 00 00"},
                {"mov eax, [edx+14]","8B 42 14"},
                {"mov [esi+b4], eax","89 86 B4 00 00 00"},
                {"mov eax,", "B8"},
                {"mov ecx,", "B9"},
                {"mov edx,", "BA"},
                {"mov ebx,", "BB"},
                {"mov esp,", "BC"},
                {"mov ebp,", "BD"},
                {"mov esi,", "BE"},
                {"mov edi,", "BF"},
                {"sub esp, esi", "2B E6"},
                {"sub esp,", "81 EC"},
                {"sub eax,", "2D"},
                {"sub ecx,", "81 E9"},
                {"sub ebp,", "81 ED"},
                {"sub edi,", "81 EF"},
                {"add eax, ecx","03 C1"},
                {"add eax, edx","03 C2"},
                {"add esp, eax","03 E0"},
                {"add esp, esi","03 E6"},
                {"add esp, ecx","03 E1"},
                {"add esp, edx","03 E2"},
                {"add edi, ebp","03 FD"},
                {"add ecx, eax","03 C8"},
                {"add ecx, edx","03 CA"},                
                {"add esp,", "81 C4"},
                {"add ecx,", "81 C1"},
                {"add edx,", "81 C2"},
                {"add esi,", "81 C6"},
                {"add edi,", "81 C7"},
                {"add dword [edx],", "81 02"},
                {"mov [eax],", "C7 00"},
                {"mov [edi],","C7 07"},
                {"add eax,", "05"},
                {"cmp eax, ecx", "3B C1"},
                {"cmp eax, esi", "3B C6"},
                {"cmp ecx,","81 F9"},
                {"cmp eax,", "3D"},
                {"cmp esi,", "81 FE"},
                {"cmp ebx,", "81 FB"},
                {"cmp edx,", "81 FA"},
                {"and ebx,", "81 E3"},
                {"and ecx,", "81 E1"},
                {"mul esi", "F7 E6"},
                {"lock add [eax], eax", "F0 01 00"},
                {"lock add [eax], eax", "F0 01 08"},
                {"lock xadd [eax], eax", "F0 0F C1 00"},
                {"lock xadd [eax], ecx", "F0 0F C1 08"},
                {"lock add dword [edx],","F0 81 02 00 00 01 00"},
                {"lock inc dword [edx]","F0 FF 02"},
                {"shl ecx,","C1 E1"},
                {"shl edi,","C1 E7"},
                {"shl eax,","C1 E0"},
                {"shr ecx,","C1 E9"},
                {"jne ", "0F 85"},
                {"je ", "0F 84"},
                {"jl ", "0F 8C"},
                {"jge ","0F 8D"},
                {"jg ","0F 8F"},
                {"retn","C2"},
                {"ret","C3"},
                {"movdfr esp,", "8B 25"},
                {"movdfr ecx,", "8B 0D"},
                {"movdfr edx,", "8B 15"},
                {"movdfr ebx,", "8B 1D"},
                {"movdfr ebp,", "8B 2D"},
                {"movdfr esi,", "8B 35"},
                {"movdfr eax,", "A1"},
                {"dec edx", "4A"},
                {"dec esi", "4E"}
            };

            // Process the assembly character by character
            int i = 0;
            bool bracketFlag = false;
            while( i < assembly.Length )
            {
                // Update the bracket flag
                if (assembly[i] == '<' || assembly[i] == '{' || assembly[i] == ';' || assembly[i] == '(' || (!bracketFlag && assembly[i] == '#'))
                    bracketFlag = true;
                else if (assembly[i] == '}' || assembly[i] == ':' || assembly[i] == '>' || assembly[i] == ')' || (bracketFlag && assembly[i] == '#'))
                    bracketFlag = false;

                if (!bracketFlag && assembly[i] != ' ' && assembly[i] != '>' && assembly[i] != '}' && assembly[i] != ':' && assembly[i] != ')' && assembly[i] != '#')
                {
                    // Perform the instruction replacement
                    bool replaced = false;
                    for (int k = 0; k <= instructions.GetUpperBound(0); k++)
                    {
                        if (i + ((string)instructions[k, 0]).Length - 1 < assembly.Length)
                        {
                            if (
                                assembly.Substring(i, ((string) instructions[k, 0]).Length).CompareTo(instructions[k, 0]) ==
                                0)
                            {
                                // Replace this instruction
                                assembly = assembly.Remove(i, ((string) instructions[k, 0]).Length);
                                assembly = assembly.Insert(i, instructions[k, 1]);
                                i += ((string) instructions[k, 1]).Length;
                                replaced = true;
                                break;
                            }
                        }
                    }

                    
                    if (!replaced)
                    {
                        // Check if it is a movdfl statement
                        if ( i + 6 - 1 < assembly.Length && assembly.Substring(i, 6).CompareTo("movdfl") == 0)
                        {
                            // Process this movdfl instruction
                            INSTRUCTION instructionDetails = processInstruction(assembly, i);

                            // Remove this movdfl instruction
                            assembly = assembly.Remove(instructionDetails.start, (instructionDetails.end - instructionDetails.start));

                            // Insert the correct byte replacement
                            switch (instructionDetails.rOp)
                            {
                                case "esp":
                                    assembly = assembly.Insert(i, "89 25" + instructionDetails.lOp);
                                    i += 5;
                                    break;
                                case "ecx":
                                    assembly = assembly.Insert(i, "89 0D" + instructionDetails.lOp);
                                    i += 5;
                                    break;
                                case "edx":
                                    assembly = assembly.Insert(i, "89 15" + instructionDetails.lOp);
                                    i += 5;
                                    break;
                                case "ebx":
                                    assembly = assembly.Insert(i, "89 1D" + instructionDetails.lOp);
                                    i += 5;
                                    break;
                                case "ebp":
                                    assembly = assembly.Insert(i, "89 2D" + instructionDetails.lOp);
                                    i += 5;
                                    break;
                                case "esi":
                                    assembly = assembly.Insert(i, "89 35" + instructionDetails.lOp);
                                    i += 5;
                                    break;
                                case "eax":
                                    assembly = assembly.Insert(i, "A3" + instructionDetails.lOp);
                                    i += 2;
                                    break;
                                default:
                                    MessageBox.Show("Failed to find movdfl register " + instructionDetails.rOp + " code at: " + assembly.Substring(i, 40));
                                    i += 0;
                                    break;
                            }
                            
                        }
                        else if (System.Uri.IsHexDigit(assembly[i]))
                        {
                            i++;
                        }else{
                            // The replacement failed, print a error message and skip to the next character
                            MessageBox.Show("Failed to parse assembly at: " + assembly.Substring(i, 100));
                            i++;
                        }
                    }
                }
                else
                {
                    i++;
                }
            }

            // Process empty spaces (4) for 4 free bytes
            int index = 0;
            while ((index = assembly.IndexOf("(")) != -1)
            {
                int endIndex = assembly.IndexOf(")");
                
                // Extract the argument
                string argument = assembly.Substring(index + 1, endIndex - index - 1);

                // Parse the integer
                int size = -1;
                if (int.TryParse(argument, out size))
                {
                    // Replace with 0's
                    string zeros = new string('0', size * 2);

                    assembly = assembly.Remove(index, endIndex - index + 1);
                    assembly = assembly.Insert(index, zeros);
                }
                else
                {
                    MessageBox.Show("Failed to process empty space descriptor (" + argument + ")");
                }
            }

            return assembly;
        }

        private static INSTRUCTION processInstruction(string instructions, int index)
        {
            string line = instructions.Substring(index);
            INSTRUCTION result;
            // Break it down into the three parameters
            result.start = index;
            result.instruction = readWord(line, ref line);
            result.lOp = readWord(line, ref line);
            result.rOp = readWord(line, ref line);
            result.end = index + (instructions.Substring(index).Length - line.Length);
            return result;
        }

        private static string readWord(string instructions, ref string rest)
        {
            // Read the next word
            string word = "";
            rest = instructions;
            while (word.Length == 0 & rest.Length != 0)
            {
                rest = rest.TrimStart(new char[] { ' ', ',' });
                word = rest.Substring(0, rest.IndexOfAny(new char[] { ' ', ',' }) );
                rest = rest.Substring(rest.IndexOfAny(new char[] { ' ', ',' }));
            }
            return word;
        }

        private static byte stringToByte(string data)
        {
            return Convert.ToByte(data, 16);
        }


        public struct INSTRUCTION
        {
            public string instruction;
            public string lOp;
            public string rOp;
            public int start;
            public int end;
        }

        [DllImport("Kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, UInt32 size, ref IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32")]
        public static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            UIntPtr nSize,
            out IntPtr lpNumberOfBytesWritten
        );

        [DllImport("Kernel32.dll")]
        public static extern Int32 VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, ref MEMORY_BASIC_INFORMATION buffer, Int32 dwLength);


        [DllImport("kernel32")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        public extern static int GetProcAddress(int hwnd, string procedureName);

        [DllImport("kernel32")]
        public extern static int LoadLibrary(string librayName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
    }
}
