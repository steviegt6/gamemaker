use std::ffi::c_void;

use windows_sys::Win32::{
    Foundation::{CloseHandle, BOOL, TRUE},
    System::{SystemServices::DLL_PROCESS_ATTACH, Threading::CreateThread},
};

#[no_mangle]
pub unsafe extern "system" fn DllMain(handle: *mut c_void, reason: u32, _: *mut u8) -> BOOL {
    if reason == DLL_PROCESS_ATTACH {
        CloseHandle(CreateThread(
            std::ptr::null(),
            0,
            Some(std::mem::transmute(
                win_main as unsafe extern "C" fn(*mut c_void) -> u32,
            )),
            handle,
            0,
            std::ptr::null_mut(),
        ));
    }

    TRUE
}

unsafe extern "C" fn win_main(_: *mut c_void) -> u32 {
    crate::main();
    0
}
