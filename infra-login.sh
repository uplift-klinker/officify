#!/usr/bin/env bash
set -xe

function azure_login() {
  az login --service-principal -u "${ARM_CLIENT_ID}" -p "${ARM_CLIENT_SECRET}" --tenant "${ARM_TENANT_ID}"
  az account set --subscription "${ARM_SUBSCRIPTION_ID}"
}

function pulumi_login() {
  pulumi login "azblob://${INFRA_STATE_CONTAINER_NAME}"
}

function main() {
  azure_login
  pulumi_login
}

main