// Copyright Discord, Inc. All Rights Reserved.

#include "DiscordUniqueID.h"

#include <string>

FDiscordUniqueID::FDiscordUniqueID()
  : Value(0)
{
}

FDiscordUniqueID::FDiscordUniqueID(uint64_t value)
  : Value(value)
{
}

FDiscordUniqueID::operator uint64_t() const
{
    return Value;
}

FString FDiscordUniqueID::ToString() const
{
    return UTF8_TO_TCHAR(std::to_string(Value).c_str());
}

FDiscordUniqueID FDiscordUniqueID::FromString(const FString& DiscordUniqueIDString, bool& Success)
{
    FDiscordUniqueID OutUniqueID;
    auto converter = FTCHARToUTF8((const TCHAR*)*DiscordUniqueIDString);
    Success = (std::from_chars((ANSICHAR*)converter.Get(),
                               (ANSICHAR*)converter.Get() + converter.Length(),
                               OutUniqueID.Value)
                 .ec == std::errc{});
    return OutUniqueID;
}
