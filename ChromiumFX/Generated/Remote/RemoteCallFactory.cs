// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

// Generated file. Do not edit.


using System;

namespace Chromium.Remote {
    internal class RemoteCallFactory {
        private delegate RemoteCall RemoteCallCtor();
        private static RemoteCallCtor[] callConstructors =  {
            () => { return new CfrMarshalAllocHGlobalRemoteCall(); },
            () => { return new CfrMarshalCopyToManagedIntPtrArrayRemoteCall(); },
            () => { return new CfrMarshalCopyToManagedRemoteCall(); },
            () => { return new CfrMarshalCopyToNativeRemoteCall(); },
            () => { return new CfrMarshalFreeHGlobalRemoteCall(); },
            () => { return new CfrMarshalPtrToStringUniRemoteCall(); },
            () => { return new CfxApiReleaseRemoteCall(); },
            () => { return new CfxAppCtorWithGCHandleRemoteCall(); },
            () => { return new CfxAppGetRenderProcessHandlerRemoteEventCall(); },
            () => { return new CfxAppGetResourceBundleHandlerRemoteEventCall(); },
            () => { return new CfxAppOnBeforeCommandLineProcessingRemoteEventCall(); },
            () => { return new CfxAppOnRegisterCustomSchemesRemoteEventCall(); },
            () => { return new CfxAppSetCallbackRemoteCall(); },
            () => { return new CfxBinaryValueCopyRemoteCall(); },
            () => { return new CfxBinaryValueCreateFromArrayRemoteCall(); },
            () => { return new CfxBinaryValueCreateRemoteCall(); },
            () => { return new CfxBinaryValueGetDataRemoteCall(); },
            () => { return new CfxBinaryValueGetSizeRemoteCall(); },
            () => { return new CfxBinaryValueIsEqualRemoteCall(); },
            () => { return new CfxBinaryValueIsOwnedRemoteCall(); },
            () => { return new CfxBinaryValueIsSameRemoteCall(); },
            () => { return new CfxBinaryValueIsValidRemoteCall(); },
            () => { return new CfxBrowserCanGoBackRemoteCall(); },
            () => { return new CfxBrowserCanGoForwardRemoteCall(); },
            () => { return new CfxBrowserGetFocusedFrameRemoteCall(); },
            () => { return new CfxBrowserGetFrameByIdentifierRemoteCall(); },
            () => { return new CfxBrowserGetFrameCountRemoteCall(); },
            () => { return new CfxBrowserGetFrameIdentifiersRemoteCall(); },
            () => { return new CfxBrowserGetFrameNamesRemoteCall(); },
            () => { return new CfxBrowserGetFrameRemoteCall(); },
            () => { return new CfxBrowserGetIdentifierRemoteCall(); },
            () => { return new CfxBrowserGetMainFrameRemoteCall(); },
            () => { return new CfxBrowserGoBackRemoteCall(); },
            () => { return new CfxBrowserGoForwardRemoteCall(); },
            () => { return new CfxBrowserHasDocumentRemoteCall(); },
            () => { return new CfxBrowserIsLoadingRemoteCall(); },
            () => { return new CfxBrowserIsPopupRemoteCall(); },
            () => { return new CfxBrowserIsSameRemoteCall(); },
            () => { return new CfxBrowserReloadIgnoreCacheRemoteCall(); },
            () => { return new CfxBrowserReloadRemoteCall(); },
            () => { return new CfxBrowserSendProcessMessageRemoteCall(); },
            () => { return new CfxBrowserStopLoadRemoteCall(); },
            () => { return new CfxCommandLineAppendArgumentRemoteCall(); },
            () => { return new CfxCommandLineAppendSwitchRemoteCall(); },
            () => { return new CfxCommandLineAppendSwitchWithValueRemoteCall(); },
            () => { return new CfxCommandLineCopyRemoteCall(); },
            () => { return new CfxCommandLineCreateRemoteCall(); },
            () => { return new CfxCommandLineGetArgumentsRemoteCall(); },
            () => { return new CfxCommandLineGetArgvRemoteCall(); },
            () => { return new CfxCommandLineGetCommandLineStringRemoteCall(); },
            () => { return new CfxCommandLineGetGlobalRemoteCall(); },
            () => { return new CfxCommandLineGetProgramRemoteCall(); },
            () => { return new CfxCommandLineGetSwitchesRemoteCall(); },
            () => { return new CfxCommandLineGetSwitchValueRemoteCall(); },
            () => { return new CfxCommandLineHasArgumentsRemoteCall(); },
            () => { return new CfxCommandLineHasSwitchesRemoteCall(); },
            () => { return new CfxCommandLineHasSwitchRemoteCall(); },
            () => { return new CfxCommandLineInitFromArgvRemoteCall(); },
            () => { return new CfxCommandLineInitFromStringRemoteCall(); },
            () => { return new CfxCommandLineIsReadOnlyRemoteCall(); },
            () => { return new CfxCommandLineIsValidRemoteCall(); },
            () => { return new CfxCommandLinePrependWrapperRemoteCall(); },
            () => { return new CfxCommandLineResetRemoteCall(); },
            () => { return new CfxCommandLineSetProgramRemoteCall(); },
            () => { return new CfxDictionaryValueClearRemoteCall(); },
            () => { return new CfxDictionaryValueCopyRemoteCall(); },
            () => { return new CfxDictionaryValueCreateRemoteCall(); },
            () => { return new CfxDictionaryValueGetBinaryRemoteCall(); },
            () => { return new CfxDictionaryValueGetBoolRemoteCall(); },
            () => { return new CfxDictionaryValueGetDictionaryRemoteCall(); },
            () => { return new CfxDictionaryValueGetDoubleRemoteCall(); },
            () => { return new CfxDictionaryValueGetIntRemoteCall(); },
            () => { return new CfxDictionaryValueGetKeysRemoteCall(); },
            () => { return new CfxDictionaryValueGetListRemoteCall(); },
            () => { return new CfxDictionaryValueGetSizeRemoteCall(); },
            () => { return new CfxDictionaryValueGetStringRemoteCall(); },
            () => { return new CfxDictionaryValueGetTypeRemoteCall(); },
            () => { return new CfxDictionaryValueGetValueRemoteCall(); },
            () => { return new CfxDictionaryValueHasKeyRemoteCall(); },
            () => { return new CfxDictionaryValueIsEqualRemoteCall(); },
            () => { return new CfxDictionaryValueIsOwnedRemoteCall(); },
            () => { return new CfxDictionaryValueIsReadOnlyRemoteCall(); },
            () => { return new CfxDictionaryValueIsSameRemoteCall(); },
            () => { return new CfxDictionaryValueIsValidRemoteCall(); },
            () => { return new CfxDictionaryValueRemoveRemoteCall(); },
            () => { return new CfxDictionaryValueSetBinaryRemoteCall(); },
            () => { return new CfxDictionaryValueSetBoolRemoteCall(); },
            () => { return new CfxDictionaryValueSetDictionaryRemoteCall(); },
            () => { return new CfxDictionaryValueSetDoubleRemoteCall(); },
            () => { return new CfxDictionaryValueSetIntRemoteCall(); },
            () => { return new CfxDictionaryValueSetListRemoteCall(); },
            () => { return new CfxDictionaryValueSetNullRemoteCall(); },
            () => { return new CfxDictionaryValueSetStringRemoteCall(); },
            () => { return new CfxDictionaryValueSetValueRemoteCall(); },
            () => { return new CfxDomDocumentGetBaseUrlRemoteCall(); },
            () => { return new CfxDomDocumentGetBodyRemoteCall(); },
            () => { return new CfxDomDocumentGetCompleteUrlRemoteCall(); },
            () => { return new CfxDomDocumentGetDocumentRemoteCall(); },
            () => { return new CfxDomDocumentGetElementByIdRemoteCall(); },
            () => { return new CfxDomDocumentGetFocusedNodeRemoteCall(); },
            () => { return new CfxDomDocumentGetHeadRemoteCall(); },
            () => { return new CfxDomDocumentGetSelectionAsMarkupRemoteCall(); },
            () => { return new CfxDomDocumentGetSelectionAsTextRemoteCall(); },
            () => { return new CfxDomDocumentGetSelectionEndOffsetRemoteCall(); },
            () => { return new CfxDomDocumentGetSelectionStartOffsetRemoteCall(); },
            () => { return new CfxDomDocumentGetTitleRemoteCall(); },
            () => { return new CfxDomDocumentGetTypeRemoteCall(); },
            () => { return new CfxDomDocumentHasSelectionRemoteCall(); },
            () => { return new CfxDomNodeGetAsMarkupRemoteCall(); },
            () => { return new CfxDomNodeGetDocumentRemoteCall(); },
            () => { return new CfxDomNodeGetElementAttributeRemoteCall(); },
            () => { return new CfxDomNodeGetElementAttributesRemoteCall(); },
            () => { return new CfxDomNodeGetElementBoundsRemoteCall(); },
            () => { return new CfxDomNodeGetElementInnerTextRemoteCall(); },
            () => { return new CfxDomNodeGetElementTagNameRemoteCall(); },
            () => { return new CfxDomNodeGetFirstChildRemoteCall(); },
            () => { return new CfxDomNodeGetFormControlElementTypeRemoteCall(); },
            () => { return new CfxDomNodeGetLastChildRemoteCall(); },
            () => { return new CfxDomNodeGetNameRemoteCall(); },
            () => { return new CfxDomNodeGetNextSiblingRemoteCall(); },
            () => { return new CfxDomNodeGetParentRemoteCall(); },
            () => { return new CfxDomNodeGetPreviousSiblingRemoteCall(); },
            () => { return new CfxDomNodeGetTypeRemoteCall(); },
            () => { return new CfxDomNodeGetValueRemoteCall(); },
            () => { return new CfxDomNodeHasChildrenRemoteCall(); },
            () => { return new CfxDomNodeHasElementAttributeRemoteCall(); },
            () => { return new CfxDomNodeHasElementAttributesRemoteCall(); },
            () => { return new CfxDomNodeIsEditableRemoteCall(); },
            () => { return new CfxDomNodeIsElementRemoteCall(); },
            () => { return new CfxDomNodeIsFormControlElementRemoteCall(); },
            () => { return new CfxDomNodeIsSameRemoteCall(); },
            () => { return new CfxDomNodeIsTextRemoteCall(); },
            () => { return new CfxDomNodeSetElementAttributeRemoteCall(); },
            () => { return new CfxDomNodeSetValueRemoteCall(); },
            () => { return new CfxDomVisitorCtorWithGCHandleRemoteCall(); },
            () => { return new CfxDomVisitorSetCallbackRemoteCall(); },
            () => { return new CfxDomVisitorVisitRemoteEventCall(); },
            () => { return new CfxFrameCopyRemoteCall(); },
            () => { return new CfxFrameCutRemoteCall(); },
            () => { return new CfxFrameDelRemoteCall(); },
            () => { return new CfxFrameExecuteJavaScriptRemoteCall(); },
            () => { return new CfxFrameGetBrowserRemoteCall(); },
            () => { return new CfxFrameGetIdentifierRemoteCall(); },
            () => { return new CfxFrameGetNameRemoteCall(); },
            () => { return new CfxFrameGetParentRemoteCall(); },
            () => { return new CfxFrameGetSourceRemoteCall(); },
            () => { return new CfxFrameGetTextRemoteCall(); },
            () => { return new CfxFrameGetUrlRemoteCall(); },
            () => { return new CfxFrameGetV8ContextRemoteCall(); },
            () => { return new CfxFrameIsFocusedRemoteCall(); },
            () => { return new CfxFrameIsMainRemoteCall(); },
            () => { return new CfxFrameIsValidRemoteCall(); },
            () => { return new CfxFrameLoadRequestRemoteCall(); },
            () => { return new CfxFrameLoadStringRemoteCall(); },
            () => { return new CfxFrameLoadUrlRemoteCall(); },
            () => { return new CfxFramePasteRemoteCall(); },
            () => { return new CfxFrameRedoRemoteCall(); },
            () => { return new CfxFrameSelectAllRemoteCall(); },
            () => { return new CfxFrameUndoRemoteCall(); },
            () => { return new CfxFrameVisitDomRemoteCall(); },
            () => { return new CfxListValueClearRemoteCall(); },
            () => { return new CfxListValueCopyRemoteCall(); },
            () => { return new CfxListValueCreateRemoteCall(); },
            () => { return new CfxListValueGetBinaryRemoteCall(); },
            () => { return new CfxListValueGetBoolRemoteCall(); },
            () => { return new CfxListValueGetDictionaryRemoteCall(); },
            () => { return new CfxListValueGetDoubleRemoteCall(); },
            () => { return new CfxListValueGetIntRemoteCall(); },
            () => { return new CfxListValueGetListRemoteCall(); },
            () => { return new CfxListValueGetSizeRemoteCall(); },
            () => { return new CfxListValueGetStringRemoteCall(); },
            () => { return new CfxListValueGetTypeRemoteCall(); },
            () => { return new CfxListValueGetValueRemoteCall(); },
            () => { return new CfxListValueIsEqualRemoteCall(); },
            () => { return new CfxListValueIsOwnedRemoteCall(); },
            () => { return new CfxListValueIsReadOnlyRemoteCall(); },
            () => { return new CfxListValueIsSameRemoteCall(); },
            () => { return new CfxListValueIsValidRemoteCall(); },
            () => { return new CfxListValueRemoveRemoteCall(); },
            () => { return new CfxListValueSetBinaryRemoteCall(); },
            () => { return new CfxListValueSetBoolRemoteCall(); },
            () => { return new CfxListValueSetDictionaryRemoteCall(); },
            () => { return new CfxListValueSetDoubleRemoteCall(); },
            () => { return new CfxListValueSetIntRemoteCall(); },
            () => { return new CfxListValueSetListRemoteCall(); },
            () => { return new CfxListValueSetNullRemoteCall(); },
            () => { return new CfxListValueSetSizeRemoteCall(); },
            () => { return new CfxListValueSetStringRemoteCall(); },
            () => { return new CfxListValueSetValueRemoteCall(); },
            () => { return new CfxLoadHandlerCtorWithGCHandleRemoteCall(); },
            () => { return new CfxLoadHandlerOnLoadEndRemoteEventCall(); },
            () => { return new CfxLoadHandlerOnLoadErrorRemoteEventCall(); },
            () => { return new CfxLoadHandlerOnLoadingStateChangeRemoteEventCall(); },
            () => { return new CfxLoadHandlerOnLoadStartRemoteEventCall(); },
            () => { return new CfxLoadHandlerSetCallbackRemoteCall(); },
            () => { return new CfxPostDataAddElementRemoteCall(); },
            () => { return new CfxPostDataCreateRemoteCall(); },
            () => { return new CfxPostDataElementCreateRemoteCall(); },
            () => { return new CfxPostDataElementGetBytesCountRemoteCall(); },
            () => { return new CfxPostDataElementGetBytesRemoteCall(); },
            () => { return new CfxPostDataElementGetFileRemoteCall(); },
            () => { return new CfxPostDataElementGetTypeRemoteCall(); },
            () => { return new CfxPostDataElementIsReadOnlyRemoteCall(); },
            () => { return new CfxPostDataElementSetToBytesRemoteCall(); },
            () => { return new CfxPostDataElementSetToEmptyRemoteCall(); },
            () => { return new CfxPostDataElementSetToFileRemoteCall(); },
            () => { return new CfxPostDataGetElementCountRemoteCall(); },
            () => { return new CfxPostDataGetElementsRemoteCall(); },
            () => { return new CfxPostDataHasExcludedElementsRemoteCall(); },
            () => { return new CfxPostDataIsReadOnlyRemoteCall(); },
            () => { return new CfxPostDataRemoveElementRemoteCall(); },
            () => { return new CfxPostDataRemoveElementsRemoteCall(); },
            () => { return new CfxProcessMessageCopyRemoteCall(); },
            () => { return new CfxProcessMessageCreateRemoteCall(); },
            () => { return new CfxProcessMessageGetArgumentListRemoteCall(); },
            () => { return new CfxProcessMessageGetNameRemoteCall(); },
            () => { return new CfxProcessMessageIsReadOnlyRemoteCall(); },
            () => { return new CfxProcessMessageIsValidRemoteCall(); },
            () => { return new CfxReadHandlerCtorWithGCHandleRemoteCall(); },
            () => { return new CfxReadHandlerEofRemoteEventCall(); },
            () => { return new CfxReadHandlerMayBlockRemoteEventCall(); },
            () => { return new CfxReadHandlerReadRemoteEventCall(); },
            () => { return new CfxReadHandlerSeekRemoteEventCall(); },
            () => { return new CfxReadHandlerSetCallbackRemoteCall(); },
            () => { return new CfxReadHandlerTellRemoteEventCall(); },
            () => { return new CfxRectCtorRemoteCall(); },
            () => { return new CfxRectDtorRemoteCall(); },
            () => { return new CfxRectGetHeightRemoteCall(); },
            () => { return new CfxRectGetWidthRemoteCall(); },
            () => { return new CfxRectGetXRemoteCall(); },
            () => { return new CfxRectGetYRemoteCall(); },
            () => { return new CfxRectSetHeightRemoteCall(); },
            () => { return new CfxRectSetWidthRemoteCall(); },
            () => { return new CfxRectSetXRemoteCall(); },
            () => { return new CfxRectSetYRemoteCall(); },
            () => { return new CfxRenderProcessHandlerCtorWithGCHandleRemoteCall(); },
            () => { return new CfxRenderProcessHandlerGetLoadHandlerRemoteEventCall(); },
            () => { return new CfxRenderProcessHandlerOnBeforeNavigationRemoteEventCall(); },
            () => { return new CfxRenderProcessHandlerOnBrowserCreatedRemoteEventCall(); },
            () => { return new CfxRenderProcessHandlerOnBrowserDestroyedRemoteEventCall(); },
            () => { return new CfxRenderProcessHandlerOnContextCreatedRemoteEventCall(); },
            () => { return new CfxRenderProcessHandlerOnContextReleasedRemoteEventCall(); },
            () => { return new CfxRenderProcessHandlerOnFocusedNodeChangedRemoteEventCall(); },
            () => { return new CfxRenderProcessHandlerOnProcessMessageReceivedRemoteEventCall(); },
            () => { return new CfxRenderProcessHandlerOnRenderThreadCreatedRemoteEventCall(); },
            () => { return new CfxRenderProcessHandlerOnUncaughtExceptionRemoteEventCall(); },
            () => { return new CfxRenderProcessHandlerOnWebKitInitializedRemoteEventCall(); },
            () => { return new CfxRenderProcessHandlerSetCallbackRemoteCall(); },
            () => { return new CfxRequestCreateRemoteCall(); },
            () => { return new CfxRequestGetFirstPartyForCookiesRemoteCall(); },
            () => { return new CfxRequestGetFlagsRemoteCall(); },
            () => { return new CfxRequestGetHeaderMapRemoteCall(); },
            () => { return new CfxRequestGetIdentifierRemoteCall(); },
            () => { return new CfxRequestGetMethodRemoteCall(); },
            () => { return new CfxRequestGetPostDataRemoteCall(); },
            () => { return new CfxRequestGetReferrerPolicyRemoteCall(); },
            () => { return new CfxRequestGetReferrerUrlRemoteCall(); },
            () => { return new CfxRequestGetResourceTypeRemoteCall(); },
            () => { return new CfxRequestGetTransitionTypeRemoteCall(); },
            () => { return new CfxRequestGetUrlRemoteCall(); },
            () => { return new CfxRequestIsReadOnlyRemoteCall(); },
            () => { return new CfxRequestSetFirstPartyForCookiesRemoteCall(); },
            () => { return new CfxRequestSetFlagsRemoteCall(); },
            () => { return new CfxRequestSetHeaderMapRemoteCall(); },
            () => { return new CfxRequestSetMethodRemoteCall(); },
            () => { return new CfxRequestSetPostDataRemoteCall(); },
            () => { return new CfxRequestSetReferrerRemoteCall(); },
            () => { return new CfxRequestSetRemoteCall(); },
            () => { return new CfxRequestSetUrlRemoteCall(); },
            () => { return new CfxResourceBundleGetDataResourceForScaleRemoteCall(); },
            () => { return new CfxResourceBundleGetDataResourceRemoteCall(); },
            () => { return new CfxResourceBundleGetGlobalRemoteCall(); },
            () => { return new CfxResourceBundleGetLocalizedStringRemoteCall(); },
            () => { return new CfxResourceBundleHandlerCtorWithGCHandleRemoteCall(); },
            () => { return new CfxResourceBundleHandlerGetDataResourceForScaleRemoteEventCall(); },
            () => { return new CfxResourceBundleHandlerGetDataResourceRemoteEventCall(); },
            () => { return new CfxResourceBundleHandlerGetLocalizedStringRemoteEventCall(); },
            () => { return new CfxResourceBundleHandlerSetCallbackRemoteCall(); },
            () => { return new CfxResponseCreateRemoteCall(); },
            () => { return new CfxResponseGetErrorRemoteCall(); },
            () => { return new CfxResponseGetHeaderMapRemoteCall(); },
            () => { return new CfxResponseGetHeaderRemoteCall(); },
            () => { return new CfxResponseGetMimeTypeRemoteCall(); },
            () => { return new CfxResponseGetStatusRemoteCall(); },
            () => { return new CfxResponseGetStatusTextRemoteCall(); },
            () => { return new CfxResponseIsReadOnlyRemoteCall(); },
            () => { return new CfxResponseSetErrorRemoteCall(); },
            () => { return new CfxResponseSetHeaderMapRemoteCall(); },
            () => { return new CfxResponseSetMimeTypeRemoteCall(); },
            () => { return new CfxResponseSetStatusRemoteCall(); },
            () => { return new CfxResponseSetStatusTextRemoteCall(); },
            () => { return new CfxRuntimeCrashReportingEnabledRemoteCall(); },
            () => { return new CfxRuntimeCreateDirectoryRemoteCall(); },
            () => { return new CfxRuntimeCreateNewTempDirectoryRemoteCall(); },
            () => { return new CfxRuntimeCreateTempDirectoryInDirectoryRemoteCall(); },
            () => { return new CfxRuntimeCurrentlyOnRemoteCall(); },
            () => { return new CfxRuntimeDeleteFileRemoteCall(); },
            () => { return new CfxRuntimeDirectoryExistsRemoteCall(); },
            () => { return new CfxRuntimeExecuteProcessRemoteCall(); },
            () => { return new CfxRuntimeFormatUrlForSecurityDisplayRemoteCall(); },
            () => { return new CfxRuntimeGetTempDirectoryRemoteCall(); },
            () => { return new CfxRuntimeIsCertStatusErrorRemoteCall(); },
            () => { return new CfxRuntimeIsCertStatusMinorErrorRemoteCall(); },
            () => { return new CfxRuntimePostDelayedTaskRemoteCall(); },
            () => { return new CfxRuntimePostTaskRemoteCall(); },
            () => { return new CfxRuntimeRegisterExtensionRemoteCall(); },
            () => { return new CfxRuntimeSetCrashKeyValueRemoteCall(); },
            () => { return new CfxRuntimeZipDirectoryRemoteCall(); },
            () => { return new CfxSchemeRegistrarAddCustomSchemeRemoteCall(); },
            () => { return new CfxStreamReaderCreateForDataRemoteCall(); },
            () => { return new CfxStreamReaderCreateForFileRemoteCall(); },
            () => { return new CfxStreamReaderCreateForHandlerRemoteCall(); },
            () => { return new CfxStreamReaderEofRemoteCall(); },
            () => { return new CfxStreamReaderMayBlockRemoteCall(); },
            () => { return new CfxStreamReaderReadRemoteCall(); },
            () => { return new CfxStreamReaderSeekRemoteCall(); },
            () => { return new CfxStreamReaderTellRemoteCall(); },
            () => { return new CfxStreamWriterCreateForFileRemoteCall(); },
            () => { return new CfxStreamWriterCreateForHandlerRemoteCall(); },
            () => { return new CfxStreamWriterFlushRemoteCall(); },
            () => { return new CfxStreamWriterMayBlockRemoteCall(); },
            () => { return new CfxStreamWriterSeekRemoteCall(); },
            () => { return new CfxStreamWriterTellRemoteCall(); },
            () => { return new CfxStreamWriterWriteRemoteCall(); },
            () => { return new CfxStringVisitorCtorWithGCHandleRemoteCall(); },
            () => { return new CfxStringVisitorSetCallbackRemoteCall(); },
            () => { return new CfxStringVisitorVisitRemoteEventCall(); },
            () => { return new CfxTaskCtorWithGCHandleRemoteCall(); },
            () => { return new CfxTaskExecuteRemoteEventCall(); },
            () => { return new CfxTaskRunnerBelongsToCurrentThreadRemoteCall(); },
            () => { return new CfxTaskRunnerBelongsToThreadRemoteCall(); },
            () => { return new CfxTaskRunnerGetForCurrentThreadRemoteCall(); },
            () => { return new CfxTaskRunnerGetForThreadRemoteCall(); },
            () => { return new CfxTaskRunnerIsSameRemoteCall(); },
            () => { return new CfxTaskRunnerPostDelayedTaskRemoteCall(); },
            () => { return new CfxTaskRunnerPostTaskRemoteCall(); },
            () => { return new CfxTaskSetCallbackRemoteCall(); },
            () => { return new CfxThreadCreateRemoteCall(); },
            () => { return new CfxThreadGetPlatformThreadIdRemoteCall(); },
            () => { return new CfxThreadGetTaskRunnerRemoteCall(); },
            () => { return new CfxThreadIsRunningRemoteCall(); },
            () => { return new CfxThreadStopRemoteCall(); },
            () => { return new CfxTimeCtorRemoteCall(); },
            () => { return new CfxTimeDtorRemoteCall(); },
            () => { return new CfxTimeGetDayOfMonthRemoteCall(); },
            () => { return new CfxTimeGetDayOfWeekRemoteCall(); },
            () => { return new CfxTimeGetHourRemoteCall(); },
            () => { return new CfxTimeGetMillisecondRemoteCall(); },
            () => { return new CfxTimeGetMinuteRemoteCall(); },
            () => { return new CfxTimeGetMonthRemoteCall(); },
            () => { return new CfxTimeGetSecondRemoteCall(); },
            () => { return new CfxTimeGetYearRemoteCall(); },
            () => { return new CfxTimeSetDayOfMonthRemoteCall(); },
            () => { return new CfxTimeSetDayOfWeekRemoteCall(); },
            () => { return new CfxTimeSetHourRemoteCall(); },
            () => { return new CfxTimeSetMillisecondRemoteCall(); },
            () => { return new CfxTimeSetMinuteRemoteCall(); },
            () => { return new CfxTimeSetMonthRemoteCall(); },
            () => { return new CfxTimeSetSecondRemoteCall(); },
            () => { return new CfxTimeSetYearRemoteCall(); },
            () => { return new CfxV8AccessorCtorWithGCHandleRemoteCall(); },
            () => { return new CfxV8AccessorGetRemoteEventCall(); },
            () => { return new CfxV8AccessorSetCallbackRemoteCall(); },
            () => { return new CfxV8AccessorSetRemoteEventCall(); },
            () => { return new CfxV8ContextEnterRemoteCall(); },
            () => { return new CfxV8ContextEvalRemoteCall(); },
            () => { return new CfxV8ContextExitRemoteCall(); },
            () => { return new CfxV8ContextGetBrowserRemoteCall(); },
            () => { return new CfxV8ContextGetCurrentContextRemoteCall(); },
            () => { return new CfxV8ContextGetEnteredContextRemoteCall(); },
            () => { return new CfxV8ContextGetFrameRemoteCall(); },
            () => { return new CfxV8ContextGetGlobalRemoteCall(); },
            () => { return new CfxV8ContextGetTaskRunnerRemoteCall(); },
            () => { return new CfxV8ContextInContextRemoteCall(); },
            () => { return new CfxV8ContextIsSameRemoteCall(); },
            () => { return new CfxV8ContextIsValidRemoteCall(); },
            () => { return new CfxV8ExceptionGetEndColumnRemoteCall(); },
            () => { return new CfxV8ExceptionGetEndPositionRemoteCall(); },
            () => { return new CfxV8ExceptionGetLineNumberRemoteCall(); },
            () => { return new CfxV8ExceptionGetMessageRemoteCall(); },
            () => { return new CfxV8ExceptionGetScriptResourceNameRemoteCall(); },
            () => { return new CfxV8ExceptionGetSourceLineRemoteCall(); },
            () => { return new CfxV8ExceptionGetStartColumnRemoteCall(); },
            () => { return new CfxV8ExceptionGetStartPositionRemoteCall(); },
            () => { return new CfxV8HandlerCtorWithGCHandleRemoteCall(); },
            () => { return new CfxV8HandlerExecuteRemoteEventCall(); },
            () => { return new CfxV8HandlerSetCallbackRemoteCall(); },
            () => { return new CfxV8InterceptorCtorWithGCHandleRemoteCall(); },
            () => { return new CfxV8InterceptorGetByIndexRemoteEventCall(); },
            () => { return new CfxV8InterceptorGetByNameRemoteEventCall(); },
            () => { return new CfxV8InterceptorSetByIndexRemoteEventCall(); },
            () => { return new CfxV8InterceptorSetByNameRemoteEventCall(); },
            () => { return new CfxV8InterceptorSetCallbackRemoteCall(); },
            () => { return new CfxV8StackFrameGetColumnRemoteCall(); },
            () => { return new CfxV8StackFrameGetFunctionNameRemoteCall(); },
            () => { return new CfxV8StackFrameGetLineNumberRemoteCall(); },
            () => { return new CfxV8StackFrameGetScriptNameOrSourceUrlRemoteCall(); },
            () => { return new CfxV8StackFrameGetScriptNameRemoteCall(); },
            () => { return new CfxV8StackFrameIsConstructorRemoteCall(); },
            () => { return new CfxV8StackFrameIsEvalRemoteCall(); },
            () => { return new CfxV8StackFrameIsValidRemoteCall(); },
            () => { return new CfxV8StackTraceGetCurrentRemoteCall(); },
            () => { return new CfxV8StackTraceGetFrameCountRemoteCall(); },
            () => { return new CfxV8StackTraceGetFrameRemoteCall(); },
            () => { return new CfxV8StackTraceIsValidRemoteCall(); },
            () => { return new CfxV8ValueAdjustExternallyAllocatedMemoryRemoteCall(); },
            () => { return new CfxV8ValueClearExceptionRemoteCall(); },
            () => { return new CfxV8ValueCreateArrayRemoteCall(); },
            () => { return new CfxV8ValueCreateBoolRemoteCall(); },
            () => { return new CfxV8ValueCreateDateRemoteCall(); },
            () => { return new CfxV8ValueCreateDoubleRemoteCall(); },
            () => { return new CfxV8ValueCreateFunctionRemoteCall(); },
            () => { return new CfxV8ValueCreateIntRemoteCall(); },
            () => { return new CfxV8ValueCreateNullRemoteCall(); },
            () => { return new CfxV8ValueCreateObjectRemoteCall(); },
            () => { return new CfxV8ValueCreateStringRemoteCall(); },
            () => { return new CfxV8ValueCreateUintRemoteCall(); },
            () => { return new CfxV8ValueCreateUndefinedRemoteCall(); },
            () => { return new CfxV8ValueDeleteValueByIndexRemoteCall(); },
            () => { return new CfxV8ValueDeleteValueByKeyRemoteCall(); },
            () => { return new CfxV8ValueExecuteFunctionRemoteCall(); },
            () => { return new CfxV8ValueExecuteFunctionWithContextRemoteCall(); },
            () => { return new CfxV8ValueGetArrayLengthRemoteCall(); },
            () => { return new CfxV8ValueGetBoolValueRemoteCall(); },
            () => { return new CfxV8ValueGetDateValueRemoteCall(); },
            () => { return new CfxV8ValueGetDoubleValueRemoteCall(); },
            () => { return new CfxV8ValueGetExceptionRemoteCall(); },
            () => { return new CfxV8ValueGetExternallyAllocatedMemoryRemoteCall(); },
            () => { return new CfxV8ValueGetFunctionHandlerRemoteCall(); },
            () => { return new CfxV8ValueGetFunctionNameRemoteCall(); },
            () => { return new CfxV8ValueGetIntValueRemoteCall(); },
            () => { return new CfxV8ValueGetKeysRemoteCall(); },
            () => { return new CfxV8ValueGetStringValueRemoteCall(); },
            () => { return new CfxV8ValueGetUintValueRemoteCall(); },
            () => { return new CfxV8ValueGetUserDataRemoteCall(); },
            () => { return new CfxV8ValueGetValueByIndexRemoteCall(); },
            () => { return new CfxV8ValueGetValueByKeyRemoteCall(); },
            () => { return new CfxV8ValueHasExceptionRemoteCall(); },
            () => { return new CfxV8ValueHasValueByIndexRemoteCall(); },
            () => { return new CfxV8ValueHasValueByKeyRemoteCall(); },
            () => { return new CfxV8ValueIsArrayRemoteCall(); },
            () => { return new CfxV8ValueIsBoolRemoteCall(); },
            () => { return new CfxV8ValueIsDateRemoteCall(); },
            () => { return new CfxV8ValueIsDoubleRemoteCall(); },
            () => { return new CfxV8ValueIsFunctionRemoteCall(); },
            () => { return new CfxV8ValueIsIntRemoteCall(); },
            () => { return new CfxV8ValueIsNullRemoteCall(); },
            () => { return new CfxV8ValueIsObjectRemoteCall(); },
            () => { return new CfxV8ValueIsSameRemoteCall(); },
            () => { return new CfxV8ValueIsStringRemoteCall(); },
            () => { return new CfxV8ValueIsUintRemoteCall(); },
            () => { return new CfxV8ValueIsUndefinedRemoteCall(); },
            () => { return new CfxV8ValueIsUserCreatedRemoteCall(); },
            () => { return new CfxV8ValueIsValidRemoteCall(); },
            () => { return new CfxV8ValueSetRethrowExceptionsRemoteCall(); },
            () => { return new CfxV8ValueSetUserDataRemoteCall(); },
            () => { return new CfxV8ValueSetValueByAccessorRemoteCall(); },
            () => { return new CfxV8ValueSetValueByIndexRemoteCall(); },
            () => { return new CfxV8ValueSetValueByKeyRemoteCall(); },
            () => { return new CfxV8ValueWillRethrowExceptionsRemoteCall(); },
            () => { return new CfxValueCopyRemoteCall(); },
            () => { return new CfxValueCreateRemoteCall(); },
            () => { return new CfxValueGetBinaryRemoteCall(); },
            () => { return new CfxValueGetBoolRemoteCall(); },
            () => { return new CfxValueGetDictionaryRemoteCall(); },
            () => { return new CfxValueGetDoubleRemoteCall(); },
            () => { return new CfxValueGetIntRemoteCall(); },
            () => { return new CfxValueGetListRemoteCall(); },
            () => { return new CfxValueGetStringRemoteCall(); },
            () => { return new CfxValueGetTypeRemoteCall(); },
            () => { return new CfxValueIsEqualRemoteCall(); },
            () => { return new CfxValueIsOwnedRemoteCall(); },
            () => { return new CfxValueIsReadOnlyRemoteCall(); },
            () => { return new CfxValueIsSameRemoteCall(); },
            () => { return new CfxValueIsValidRemoteCall(); },
            () => { return new CfxValueSetBinaryRemoteCall(); },
            () => { return new CfxValueSetBoolRemoteCall(); },
            () => { return new CfxValueSetDictionaryRemoteCall(); },
            () => { return new CfxValueSetDoubleRemoteCall(); },
            () => { return new CfxValueSetIntRemoteCall(); },
            () => { return new CfxValueSetListRemoteCall(); },
            () => { return new CfxValueSetNullRemoteCall(); },
            () => { return new CfxValueSetStringRemoteCall(); },
            () => { return new CfxWaitableEventCreateRemoteCall(); },
            () => { return new CfxWaitableEventIsSignaledRemoteCall(); },
            () => { return new CfxWaitableEventResetRemoteCall(); },
            () => { return new CfxWaitableEventSignalRemoteCall(); },
            () => { return new CfxWaitableEventTimedWaitRemoteCall(); },
            () => { return new CfxWaitableEventWaitRemoteCall(); },
            () => { return new CfxWriteHandlerCtorWithGCHandleRemoteCall(); },
            () => { return new CfxWriteHandlerFlushRemoteEventCall(); },
            () => { return new CfxWriteHandlerMayBlockRemoteEventCall(); },
            () => { return new CfxWriteHandlerSeekRemoteEventCall(); },
            () => { return new CfxWriteHandlerSetCallbackRemoteCall(); },
            () => { return new CfxWriteHandlerTellRemoteEventCall(); },
            () => { return new CfxWriteHandlerWriteRemoteEventCall(); },
            () => { return new CfxXmlReaderCloseRemoteCall(); },
            () => { return new CfxXmlReaderCreateRemoteCall(); },
            () => { return new CfxXmlReaderGetAttributeByIndexRemoteCall(); },
            () => { return new CfxXmlReaderGetAttributeByLNameRemoteCall(); },
            () => { return new CfxXmlReaderGetAttributeByQNameRemoteCall(); },
            () => { return new CfxXmlReaderGetAttributeCountRemoteCall(); },
            () => { return new CfxXmlReaderGetBaseUriRemoteCall(); },
            () => { return new CfxXmlReaderGetDepthRemoteCall(); },
            () => { return new CfxXmlReaderGetErrorRemoteCall(); },
            () => { return new CfxXmlReaderGetInnerXmlRemoteCall(); },
            () => { return new CfxXmlReaderGetLineNumberRemoteCall(); },
            () => { return new CfxXmlReaderGetLocalNameRemoteCall(); },
            () => { return new CfxXmlReaderGetNamespaceUriRemoteCall(); },
            () => { return new CfxXmlReaderGetOuterXmlRemoteCall(); },
            () => { return new CfxXmlReaderGetPrefixRemoteCall(); },
            () => { return new CfxXmlReaderGetQualifiedNameRemoteCall(); },
            () => { return new CfxXmlReaderGetTypeRemoteCall(); },
            () => { return new CfxXmlReaderGetValueRemoteCall(); },
            () => { return new CfxXmlReaderGetXmlLangRemoteCall(); },
            () => { return new CfxXmlReaderHasAttributesRemoteCall(); },
            () => { return new CfxXmlReaderHasErrorRemoteCall(); },
            () => { return new CfxXmlReaderHasValueRemoteCall(); },
            () => { return new CfxXmlReaderIsEmptyElementRemoteCall(); },
            () => { return new CfxXmlReaderMoveToAttributeByIndexRemoteCall(); },
            () => { return new CfxXmlReaderMoveToAttributeByLNameRemoteCall(); },
            () => { return new CfxXmlReaderMoveToAttributeByQNameRemoteCall(); },
            () => { return new CfxXmlReaderMoveToCarryingElementRemoteCall(); },
            () => { return new CfxXmlReaderMoveToFirstAttributeRemoteCall(); },
            () => { return new CfxXmlReaderMoveToNextAttributeRemoteCall(); },
            () => { return new CfxXmlReaderMoveToNextNodeRemoteCall(); },
            () => { return new CfxZipReaderCloseFileRemoteCall(); },
            () => { return new CfxZipReaderCloseRemoteCall(); },
            () => { return new CfxZipReaderCreateRemoteCall(); },
            () => { return new CfxZipReaderEofRemoteCall(); },
            () => { return new CfxZipReaderGetFileLastModifiedRemoteCall(); },
            () => { return new CfxZipReaderGetFileNameRemoteCall(); },
            () => { return new CfxZipReaderGetFileSizeRemoteCall(); },
            () => { return new CfxZipReaderMoveToFileRemoteCall(); },
            () => { return new CfxZipReaderMoveToFirstFileRemoteCall(); },
            () => { return new CfxZipReaderMoveToNextFileRemoteCall(); },
            () => { return new CfxZipReaderOpenFileRemoteCall(); },
            () => { return new CfxZipReaderReadFileRemoteCall(); },
            () => { return new CfxZipReaderTellRemoteCall(); },
            () => { return new ExecuteProcessRemoteCall(); },
            () => { return new FreeGCHandleRemoteCall(); },
        } ;

        internal static RemoteCall ForCallId(RemoteCallId id) {
            return callConstructors[(int)id]();
        }
    }
}
