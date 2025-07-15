import React, {useEffect, useState} from 'react';
import { Admin, Loading, Resource, ResourceElement } from 'react-admin';
import odataProvider, { OdataDataProvider } from "ra-data-odata-server";
import UserIcon from "@mui/icons-material/People";
import StarIcon from "@mui/icons-material/Star";
import SchoolIcon from "@mui/icons-material/School";

import { UsersList, UserCreate, UserEdit, UserShow } from "./services/users";
import {
  ExperienceList,
  ExperienceShow,
  ExperienceEdit,
  ExperienceCreate
} from "./services/experience";
import {
  EducationList,
  EducationShow,
  EducationCreate,
  EducationEdit
} from "./services/education";
import { PortofolioList, PortofolioShow, PortofolioUpdate, PortofolioCreate } from "./services/portofolio";
import {
  BlogList,
  BlogShow,
  BlogCreate,
  BlogEdit
} from "./services/blog";
import "./App.css";
// import Dashboard from "./Dashboard";

const resourceList = (permissions: any) => [
  permissions === "admin" ? (
    <Resource
      name="users"
      list={UsersList}
      create={UserCreate}
      edit={UserEdit}
      icon={UserIcon}
      show={UserShow}
    />
  ) : null,
  (
    <Resource
      name="experiences"
      icon={StarIcon}
      list={ExperienceList}
      show={ExperienceShow}
      edit={ExperienceEdit}
      create={ExperienceCreate}
    />
  ),
  permissions === "admin" ? (
    <Resource
      name="education"
      icon={SchoolIcon}
      list={EducationList}
      show={EducationShow}
      edit={EducationEdit}
      create={EducationCreate}
    />
  ) : null,
  permissions === "admin" ? (
    <Resource
      name="portofolio"
      list={PortofolioList}
      show={PortofolioShow}
      edit={PortofolioUpdate}
      create={PortofolioCreate}
    />
  ) : null,
  permissions === "admin" ? (
    <Resource
      name="blogs"
      list={BlogList}
      show={BlogShow}
      edit={BlogEdit}
      create={BlogCreate}
    />
  ) : null
].filter((element) => Boolean(element)).map(element => element as React.JSX.Element);

function App() {
  const [dataProvider, setDataProvider] = useState<OdataDataProvider>();
  useEffect(() => {
    odataProvider(
        "/api"
    ).then((p) => setDataProvider(p));
    return () => {
    };
  }, []);

  return (
      dataProvider ?
          <Admin title="Berv Admin" dataProvider={dataProvider}>
            {resourceList}
          </Admin> : (
              <Loading></Loading>
          )
  );
}

export default App;
