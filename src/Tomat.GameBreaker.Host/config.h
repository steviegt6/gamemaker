#pragma once

#include <string>

#include "json.hpp"

nlohmann::json* init_config(const std::wstring& cwd, const std::wstring& managed_host_dir);
