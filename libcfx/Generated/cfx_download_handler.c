// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

// Generated file. Do not edit.


// cef_download_handler

typedef struct _cfx_download_handler_t {
    cef_download_handler_t cef_download_handler;
    unsigned int ref_count;
    gc_handle_t gc_handle;
    int wrapper_kind;
    // managed callbacks
    void (CEF_CALLBACK *on_before_download)(gc_handle_t self, cef_browser_t* browser, int *browser_release, cef_download_item_t* download_item, int *download_item_release, char16 *suggested_name_str, int suggested_name_length, cef_before_download_callback_t* callback, int *callback_release);
    void (CEF_CALLBACK *on_download_updated)(gc_handle_t self, cef_browser_t* browser, int *browser_release, cef_download_item_t* download_item, int *download_item_release, cef_download_item_callback_t* callback, int *callback_release);
} cfx_download_handler_t;

void CEF_CALLBACK _cfx_download_handler_add_ref(struct _cef_base_ref_counted_t* base) {
    InterlockedIncrement(&((cfx_download_handler_t*)base)->ref_count);
}
int CEF_CALLBACK _cfx_download_handler_release(struct _cef_base_ref_counted_t* base) {
    int count = InterlockedDecrement(&((cfx_download_handler_t*)base)->ref_count);
    if(count == 0) {
        if(((cfx_download_handler_t*)base)->wrapper_kind == 0) {
            cfx_gc_handle_switch(&((cfx_download_handler_t*)base)->gc_handle, GC_HANDLE_FREE);
        } else {
            cfx_gc_handle_switch(&((cfx_download_handler_t*)base)->gc_handle, GC_HANDLE_FREE | GC_HANDLE_REMOTE);
        }
        free(base);
        return 1;
    }
    return 0;
}
int CEF_CALLBACK _cfx_download_handler_has_one_ref(struct _cef_base_ref_counted_t* base) {
    return ((cfx_download_handler_t*)base)->ref_count == 1 ? 1 : 0;
}

static cfx_download_handler_t* cfx_download_handler_ctor(gc_handle_t gc_handle, int wrapper_kind) {
    cfx_download_handler_t* ptr = (cfx_download_handler_t*)calloc(1, sizeof(cfx_download_handler_t));
    if(!ptr) return 0;
    ptr->cef_download_handler.base.size = sizeof(cef_download_handler_t);
    ptr->cef_download_handler.base.add_ref = _cfx_download_handler_add_ref;
    ptr->cef_download_handler.base.release = _cfx_download_handler_release;
    ptr->cef_download_handler.base.has_one_ref = _cfx_download_handler_has_one_ref;
    ptr->ref_count = 1;
    ptr->gc_handle = gc_handle;
    ptr->wrapper_kind = wrapper_kind;
    return ptr;
}

// on_before_download

void CEF_CALLBACK cfx_download_handler_on_before_download(cef_download_handler_t* self, cef_browser_t* browser, cef_download_item_t* download_item, const cef_string_t* suggested_name, cef_before_download_callback_t* callback) {
    int browser_release;
    int download_item_release;
    int callback_release;
    ((cfx_download_handler_t*)self)->on_before_download(((cfx_download_handler_t*)self)->gc_handle, browser, &browser_release, download_item, &download_item_release, suggested_name ? suggested_name->str : 0, suggested_name ? (int)suggested_name->length : 0, callback, &callback_release);
    if(browser_release) browser->base.release((cef_base_ref_counted_t*)browser);
    if(download_item_release) download_item->base.release((cef_base_ref_counted_t*)download_item);
    if(callback_release) callback->base.release((cef_base_ref_counted_t*)callback);
}

// on_download_updated

void CEF_CALLBACK cfx_download_handler_on_download_updated(cef_download_handler_t* self, cef_browser_t* browser, cef_download_item_t* download_item, cef_download_item_callback_t* callback) {
    int browser_release;
    int download_item_release;
    int callback_release;
    ((cfx_download_handler_t*)self)->on_download_updated(((cfx_download_handler_t*)self)->gc_handle, browser, &browser_release, download_item, &download_item_release, callback, &callback_release);
    if(browser_release) browser->base.release((cef_base_ref_counted_t*)browser);
    if(download_item_release) download_item->base.release((cef_base_ref_counted_t*)download_item);
    if(callback_release) callback->base.release((cef_base_ref_counted_t*)callback);
}

static void cfx_download_handler_set_callback(cef_download_handler_t* self, int index, void* callback) {
    switch(index) {
    case 0:
        ((cfx_download_handler_t*)self)->on_before_download = (void (CEF_CALLBACK *)(gc_handle_t self, cef_browser_t* browser, int *browser_release, cef_download_item_t* download_item, int *download_item_release, char16 *suggested_name_str, int suggested_name_length, cef_before_download_callback_t* callback, int *callback_release))callback;
        self->on_before_download = callback ? cfx_download_handler_on_before_download : 0;
        break;
    case 1:
        ((cfx_download_handler_t*)self)->on_download_updated = (void (CEF_CALLBACK *)(gc_handle_t self, cef_browser_t* browser, int *browser_release, cef_download_item_t* download_item, int *download_item_release, cef_download_item_callback_t* callback, int *callback_release))callback;
        self->on_download_updated = callback ? cfx_download_handler_on_download_updated : 0;
        break;
    }
}

