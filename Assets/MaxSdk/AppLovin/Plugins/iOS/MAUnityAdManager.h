//
//  MAUnityAdManager.h
//  AppLovin MAX Unity Plugin
//

#import <Foundation/Foundation.h>
#import <AppLovinSDK/AppLovinSDK.h>

NS_ASSUME_NONNULL_BEGIN
typedef const void *MAUnityRef;
typedef void (*ALUnityBackgroundCallback)(const char* args);

@interface MAUnityAdManager : NSObject

- (ALSdk *)initializeSdkWithAdUnitIdentifiers:(NSString *)serializedAdUnitIdentifiers metaData:(NSString *)serializedMetaData backgroundCallback:(ALUnityBackgroundCallback)unityBackgroundCallback andCompletionHandler:(ALSdkInitializationCompletionHandler)completionHandler;

- (void)createBannerWithAdUnitIdentifier:(NSString *)adUnitIdentifier atPosition:(NSString *)bannerPosition;
- (void)createBannerWithAdUnitIdentifier:(NSString *)adUnitIdentifier x:(CGFloat)xOffset y:(CGFloat)yOffset;
- (void)setBannerBackgroundColorForAdUnitIdentifier:(NSString *)adUnitIdentifier hexColorCode:(NSString *)hexColorCode;
- (void)setBannerPlacement:(nullable NSString *)placement forAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)startBannerAutoRefreshForAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)stopBannerAutoRefreshForAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)setBannerExtraParameterForAdUnitIdentifier:(NSString *)adUnitIdentifier key:(NSString *)key value:(nullable NSString *)value;
- (void)setBannerLocalExtraParameterForAdUnitIdentifier:(NSString *)adUnitIdentifier key:(NSString *)key value:(nullable id)value;
- (void)setBannerCustomPostbackData:(nullable NSString *)value forAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)setBannerWidth:(CGFloat)width forAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)updateBannerPosition:(NSString *)bannerPosition forAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)updateBannerPosition:(CGFloat)xOffset y:(CGFloat)yOffset forAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)showBannerWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)destroyBannerWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)hideBannerWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (NSString *)bannerLayoutForAdUnitIdentifier:(NSString *)adUnitIdentifier;
+ (CGFloat)adaptiveBannerHeightForWidth:(CGFloat)width;

- (void)createMRecWithAdUnitIdentifier:(NSString *)adUnitIdentifier atPosition:(NSString *)mrecPosition;
- (void)createMRecWithAdUnitIdentifier:(NSString *)adUnitIdentifier x:(CGFloat)xOffset y:(CGFloat)yOffset;
- (void)setMRecPlacement:(nullable NSString *)placement forAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)startMRecAutoRefreshForAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)stopMRecAutoRefreshForAdUnitIdentifier:(NSString *)adUnitIdentifer;
- (void)setMRecExtraParameterForAdUnitIdentifier:(NSString *)adUnitIdentifier key:(NSString *)key value:(nullable NSString *)value;
- (void)setMRecLocalExtraParameterForAdUnitIdentifier:(NSString *)adUnitIdentifier key:(NSString *)key value:(nullable id)value;
- (void)setMRecCustomPostbackData:(nullable NSString *)value forAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)showMRecWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)destroyMRecWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)hideMRecWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)updateMRecPosition:(NSString *)mrecPosition forAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)updateMRecPosition:(CGFloat)xOffset y:(CGFloat)yOffset forAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (NSString *)mrecLayoutForAdUnitIdentifier:(NSString *)adUnitIdentifier;

- (void)createCrossPromoAdWithAdUnitIdentifier:(NSString *)adUnitIdentifier x:(CGFloat)xOffset y:(CGFloat)yOffset width:(CGFloat)width height:(CGFloat)height rotation:(CGFloat)rotation;
- (void)setCrossPromoAdPlacement:(nullable NSString *)placement forAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)showCrossPromoAdWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)destroyCrossPromoAdWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)hideCrossPromoAdWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)updateCrossPromoAdPositionForAdUnitIdentifier:(NSString *)adUnitIdentifier x:(CGFloat)xOffset y:(CGFloat)yOffset width:(CGFloat)width height:(CGFloat)height rotation:(CGFloat)rotation;
- (NSString *)crossPromoAdLayoutForAdUnitIdentifier:(NSString *)adUnitIdentifier;

- (void)loadInterstitialWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (BOOL)isInterstitialReadyWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)showInterstitialWithAdUnitIdentifier:(NSString *)adUnitIdentifier placement:(NSString *)placement;
- (void)setInterstitialExtraParameterForAdUnitIdentifier:(NSString *)adUnitIdentifier key:(NSString *)key value:(nullable NSString *)value;
- (void)setInterstitialLocalExtraParameterForAdUnitIdentifier:(NSString *)adUnitIdentifier key:(NSString *)key value:(nullable id)value;
- (void)setInterstitialCustomPostbackData:(nullable NSString *)value forAdUnitIdentifier:(NSString *)adUnitIdentifier;

- (void)loadRewardedAdWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (BOOL)isRewardedAdReadyWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)showRewardedAdWithAdUnitIdentifier:(NSString *)adUnitIdentifier placement:(NSString *)placement;
- (void)setRewardedAdExtraParameterForAdUnitIdentifier:(NSString *)adUnitIdentifier key:(NSString *)key value:(nullable NSString *)value;
- (void)setRewardedAdLocalExtraParameterForAdUnitIdentifier:(NSString *)adUnitIdentifier key:(NSString *)key value:(nullable id)value;
- (void)setRewardedAdCustomPostbackData:(nullable NSString *)value forAdUnitIdentifier:(NSString *)adUnitIdentifier;

- (void)loadRewardedInterstitialAdWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (BOOL)isRewardedInterstitialAdReadyWithAdUnitIdentifier:(NSString *)adUnitIdentifier;
- (void)showRewardedInterstitialAdWithAdUnitIdentifier:(NSString *)adUnitIdentifier placement:(NSString *)placement;
- (void)setRewardedInterstitialAdExtraParameterForAdUnitIdentifier:(NSString *)adUnitIdentifier key:(NSString *)key value:(nullable NSString *)value;
- (void)setRewardedInterstitialAdLocalExtraParameterForAdUnitIdentifier:(NSString *)adUnitIdentifier key:(NSString *)key value:(nullable id)value;
- (void)setRewardedInterstitialAdCustomPostbackData:(nullable NSString *)value forAdUnitIdentifier:(NSString *)adUnitIdentifier;

// Event Tracking
- (void)trackEvent:(NSString *)event parameters:(NSString *)parameters;

// Ad Info
- (NSString *)adInfoForAdUnitIdentifier:(NSString *)adUnitIdentifier;

// Ad Value
- (NSString *)adValueForAdUnitIdentifier:(NSString *)adUnitIdentifier withKey:(NSString *)key;

// User Service
- (void)didDismissUserConsentDialog;

// Utils
+ (NSString *)serializeParameters:(NSDictionary<NSString *, NSString *> *)dict;

/**
 * Creates an instance of @c MAUnityAdManager if needed and returns the singleton instance.
 */
+ (instancetype)shared;

- (instancetype)init NS_UNAVAILABLE;

@end

@interface MAUnityAdManager(ALDeprecated)
- (void)loadVariables __deprecated_msg("This API has been deprecated. Please use our SDK's initialization callback to retrieve variables instead.");
@end

NS_ASSUME_NONNULL_END
