#pragma once

#include <string>
#include "include/json.hpp"

nlohmann::json* init_config(const std::wstring& cwd);
