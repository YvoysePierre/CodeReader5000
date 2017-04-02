using System;

public class loginVerification
{
    //  Login for server
    String service = "http://";

    public object Debug { get; private set; }

    public void Verification()
    {

        // Downloading .NET Framework reference to figure out to resolve the issues in red here. 
        string user = "username";
        string pwd = "plainpassword";
        string token = GetSessionToken(user, pwd);

        if
            (string.IsNullOrEmpty(token))
            using System.Text; 
        {
            using System.Text;
            Debug.WriteLine("Login Failed");
        }
        else
        {
            Debug.WriteLine("Login Successful");
 
            int iClient = 2;
            string clientPropsService = service + "client/" + iClient;
            HttpWebResponse ClientResp = SendRequest(clientService, "GET", token, null);

            if 
                (ClientResp.StatusCode == HttpStatusCode.OK)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ClientResp.GetResponseStream());
                
                //Parse response to get client name, host name and client description
                Debug.WriteLine(string.Format("Client properties response: ", xmlDoc.InnerXml));
                string clientName = xmlDoc.SelectSingleNode("/App_GetClientPropertiesResponse/clientProperties/client/clientEntity/@clientName").Value;
                string clientHostName = xmlDoc.SelectSingleNode("/App_GetClientPropertiesResponse/clientProperties/client/clientEntity/@hostName").Value;
                string clientDescription = xmlDoc.SelectSingleNode("/App_GetClientPropertiesResponse/clientProperties/client/@clientDescription").Value;
            }

            else

            {

                Debug.WriteLine(string.Format("Get Client properties request Failed. Status Code: {0}, Status Description: {1}", ClientResp.StatusCode, ClientResp.StatusDescription));

            }

            //3. Set client props
            //The following request XML is hard coded here but can be read from a file and appropriate properties set.
            string newJobPriority = "7";
            string updateClientProps = "<App_SetClientPropertiesRequest><clientProperties><clientProps JobPriority=\"<<jobPriority>>\"></clientProps></clientProperties></App_SetClientPropertiesRequest>";
            updateClientProps = updateClientProps.Replace("<<jobPriority>>", newJobPriority);
            HttpWebResponse clientUpdateResp = SendRequest(clientService, "POST", token, updateClientProps);
            if (ClientResp.StatusCode == HttpStatusCode.OK)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(clientUpdateResp.GetResponseStream());
                Debug.WriteLine(string.Format("Client properties response: ", xmlDoc.InnerXml));
                string errorCode = xmlDoc.SelectSingleNode("/App_SetClientPropertiesResponse/response/@errorCode").Value;
                if (errorCode == "0")
                {
                    Debug.WriteLine("Client properties set successfully");
                }
                else
                {
                    Debug.WriteLine("Client properties could not be set. Error Code: " + errorCode);
                }
            }
            else
            {
                Debug.WriteLine(string.Format("Set client properties request Failed. Status Code: {0}, Status Description: {1}", ClientResp.StatusCode, ClientResp.StatusDescription));
            }

        }
    }

 